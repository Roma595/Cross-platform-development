import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { CourseService } from '../../data/services/course.service';
import { Course, CoursesViewMode} from '../../data/interfaces/course.interface';
import { CourseCard } from '../../common-ui/course-card/course-card';
import { FormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { TeacherService } from '../../data/services/teacher.service';
import { StudentService } from '../../data/services/student.service';

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
  studentService = inject(StudentService);
  
  isCreateOpen = false;

  courses = signal<Course[]>([]);
  teachers: Map<number, string> = new Map();
  takenPlacesByCourse: Map<number, number> = new Map();

  teacherSearch = '';
  filteredTeachers: { id: number; fullName: string }[] = [];
  selectedTeacherId: number | null = null;

  editingCourse = signal<Course | null>(null);
  countTakePlaces = signal<number>(0);
  searchQuery = '';

  viewMode: CoursesViewMode = 'cards';

  sortColumn: string | null = null;
  sortDirection: 'asc' | 'desc' = 'asc';

  ngOnInit(): void {
    this.loadCourses();
    if (this.cookieService.get('role') === 'Admin'){
      this.loadTeachers();
    }
    
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
    this.getTakenPlaces(course.id);
  }

  closeCreateDialog() {
    this.isCreateOpen = false;
    this.editingCourse.set(null);
    this.teacherSearch = "";
    this.courses.set([]);
    this.loadCourses();
  }

  loadCourses(): void {
    this.courseService.getAllCourses().subscribe({
      next: (data: Course[]) => {
        this.courses.set(data);
        this.fillTakenPlacesMap(data);  
      },
      error: () => {
        console.log('error get all courses');
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

  fillTakenPlacesMap(courses: Course[]) {
    this.takenPlacesByCourse.clear();

    courses.forEach(course => {
      this.studentService.getAllStudentsByCourseId(course.id).subscribe({
        next: students => {
          this.takenPlacesByCourse.set(course.id, students.length);
        },
        error: () => {
          this.takenPlacesByCourse.set(course.id, 0);
        }
      });
    });
  }

  getTakenPlaces(courseId: number): number {
    return this.takenPlacesByCourse.get(courseId) ?? 0;
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
    this.teacherSearch = option.fullName;
    this.filteredTeachers = [];
  }

  getTeacherName(teacherId: number){
    return this.teachers.get(teacherId) ?? '';
  }

  submitCreateorEdit(formValue: any) {
    const editing = this.editingCourse();

    const teacherId = this.selectedTeacherId;

    if (!teacherId) {
      alert('Выберите преподавателя из списка');
      return;
    }

    if (!editing) {
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
        error: () => {
          console.log('error create course');
        }
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
        error: () => {
          console.log('error update course');
        }
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
      },
      error: () => {
        console.log('error delete course');
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
