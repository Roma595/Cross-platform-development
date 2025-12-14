import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { CourseService } from '../../data/services/course.service';
import { Course, CoursesViewMode} from '../../data/interfaces/course.interface';
import { CourseCard } from '../../common-ui/course-card/course-card';
import { FormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { TeacherService } from '../../data/services/teacher.service';

@Component({
  selector: 'app-courses-page',
  imports: [CourseCard, FormsModule],
  templateUrl: './courses-page.html',
  styleUrl: './courses-page.scss',
})

export class CoursesPage {
  courseService = inject(CourseService);
  cookieService = inject(CookieService);
  teacherService = inject(TeacherService);
  
  isCreateOpen = false;

  courses = signal<Course[]>([]);
  teachers: Map<number, string> = new Map();
  teacherSearch = '';
  filteredTeachers: { id: number; fullName: string }[] = [];
  selectedTeacherId: number | null = null;
  loading = signal(true);
  editingCourse = signal<Course | null>(null);

  searchQuery = '';

  viewMode: CoursesViewMode = 'cards';

  sortColumn: string | null = null;
  sortDirection: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
    this.loadCourses();
    this.loadTeachers()
  }

  setViewMode(mode: CoursesViewMode) {
    this.viewMode = mode;
  }

  openCreateDialog() {
    this.isCreateOpen = true;
  }

  openEditDialog(course: Course) {
    this.editingCourse.set(course);
    this.isCreateOpen = true; 
    this.selectedTeacherId = course.teacherId;
    this.teacherSearch = this.getTeacherName(course.teacherId) || '';
  }

  closeCreateDialog() {
    this.isCreateOpen = false;
    this.editingCourse.set(null);
    this.teacherSearch = "";
  }

  loadCourses(): void {
    this.courseService.getAllCourses().subscribe({
      next: (data: Course[]) => {
        this.courses.set(data);  
        this.loading.set(false);
      },
      error: err => {
        this.loading.set(false);
      }
    });
  }

  loadTeachers() {
    this.teacherService.getAllTeachers().subscribe(data => {
      data.forEach(teacher => {
        const fullName = `${teacher.firstName} ${teacher.lastName}`;
        this.teachers.set(teacher.id, fullName);
      });
      this.filteredTeachers = Array.from(this.teachers.entries()).map(
        ([id, fullName]) => ({ id, fullName })
      );
    });
  }

  onTeacherSearchChange(value: string) {
    this.teacherSearch = value;
    const term = value.toLowerCase().trim();

    this.filteredTeachers = Array.from(this.teachers.entries())
      .map(([id, fullName]) => ({ id, fullName }))
      .filter(t => t.fullName.toLowerCase().includes(term));
  }

  selectTeacher(option: { id: number; fullName: string }) {
    this.selectedTeacherId = option.id;
    this.teacherSearch = option.fullName;       // показываем ФИО
    this.filteredTeachers = [];                // прячем список
  }
  getTeacherName(teacherId: number){
    return this.teachers.get(teacherId);
  }

  submitCreateorEdit(formValue: any) {
    const editing = this.editingCourse();

    const teacherId = this.selectedTeacherId;

    if (!teacherId) {
      alert('Выберите преподавателя из списка');
      return;
    }

    if (!editing) {
      // СОЗДАНИЕ
      const newCourse: Course = {
        id: 0,
        teacherId: teacherId,
        name: formValue.name,
        totalPlaces: formValue.totalPlaces,
        startDate: formValue.startDate,
        endDate: formValue.endDate,
      };

      this.courseService.createCourse(newCourse).subscribe({
        next: course => {
          this.courses.update(list => [...list, course]);
          this.closeCreateDialog();
        },
      });

    } else {
      const updated: Course = {
        id: editing.id,
        teacherId: teacherId,
        name: formValue.name,
        totalPlaces: formValue.totalPlaces,
        startDate: formValue.startDate,
        endDate: formValue.endDate,
      };

      this.courseService.updateCourse(updated.id, updated).subscribe({
        next: () => {
          this.loadCourses();
          this.closeCreateDialog();
        },
      });
    }
    this.teacherSearch = "";
  }

  deleteCourse(id: number){
    const ok = window.confirm('Вы точно хотите удалить курс?');
    if (!ok) {
      return;
    }
    this.courseService.deleteCourse(id).subscribe({
      next: () => {
        this.courses.update(list => list.filter(c => c.id !== id));
      }
    });
  }

  formatDate(date: string | Date): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('ru-RU');
  }

  sort(column: keyof Course) {
    // если кликаем на ту же колонку — меняем направление
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // новая колонка — сортируем по возрастанию
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    // сортируем массив
    this.courses().sort((a, b) => {
      const aVal = a[column];
      const bVal = b[column];

      // если числа
      if (typeof aVal === 'number' && typeof bVal === 'number') {
        return this.sortDirection === 'asc' ? aVal - bVal : bVal - aVal;
      }

      // если строки
      const aStr = String(aVal).toLowerCase();
      const bStr = String(bVal).toLowerCase();
      const comparison = aStr.localeCompare(bStr, 'ru');
      return this.sortDirection === 'asc' ? comparison : -comparison;
    });
  }

  getSortIcon(column: string): string {
    if (this.sortColumn !== column) return '↕';
    return this.sortDirection === 'asc' ? '↑' : '↓';
  }

  getFilteredCourses(): Course[] {
    if (!this.searchQuery.trim()) {
      return this.courses();
    }

    const query = this.searchQuery.toLowerCase().trim();

    return this.courses().filter(course => {
      const courseName = course.name.toLowerCase();
      const teacherName = (this.getTeacherName(course.teacherId) || '').toLowerCase();
      
      return courseName.includes(query) || teacherName.includes(query);
    });
  }

  onSearchChange(value: string) {
    this.searchQuery = value;
  }

  clearSearch() {
    this.searchQuery = '';
  }
}
