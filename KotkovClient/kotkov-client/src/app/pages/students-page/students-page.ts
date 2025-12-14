import { Component, inject, signal } from '@angular/core';
import { StudentService } from '../../data/services/student.service';
import { Profile } from '../../data/interfaces/profile.interface';
import { ProfileCard } from '../../common-ui/profile-card/profile-card';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../data/services/course.service';

@Component({
  selector: 'app-students-page',
  imports: [ProfileCard, FormsModule],
  templateUrl: './students-page.html',
  styleUrl: './students-page.scss',
})
export class StudentsPage {
  studentService = inject(StudentService);
  courseService = inject(CourseService);

  students = signal<Profile[]>([]);
  isCreateOpen = false;
  editingStudent = signal<Profile | null>(null);

  // для поиска
  searchQuery = '';

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (data: Profile[]) => {
        this.students.set(data);

        data.forEach(student => {
        this.courseService.getAllCoursesForStudent(student.id).subscribe(
          courses => {
            student.courses = courses;
            this.students.update(list => [...list]);
          }
        );
      });
      },
      error: () => {
      }
    });
  }

  openCreateDialog() {
    this.editingStudent.set(null);
    this.isCreateOpen = true;
  }

  openEditDialog(student: Profile) {
    this.editingStudent.set(student);
    this.isCreateOpen = true;
  }

  closeCreateDialog() {
    this.isCreateOpen = false;
    this.editingStudent.set(null);
  }

  submitCreateOrEdit(formValue: any) {
    const editing = this.editingStudent();

    if (!editing) {
      // СОЗДАНИЕ
      const newStudent: Profile = {
        id: 0,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phoneNumber: formValue.phone,
        role: "Student",
        courses: []
      };

      this.studentService.createStudent(newStudent).subscribe({
        next: student => {
          this.students.update(list => [...list, student]);
          this.closeCreateDialog();
        },
      });
    } else {
      // РЕДАКТИРОВАНИЕ
      const updated: Profile = {
        id: editing.id,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phoneNumber: formValue.phone,
        role: 'Student',
        courses: editing.courses
      };

      this.studentService.updateStudent(updated.id, updated).subscribe({
        next: () => {
          this.loadStudents();
          this.closeCreateDialog();
        },
      });
    }
  }

  deleteStudent(id: number) {
    const ok = window.confirm('Вы точно хотите удалить студента?');
    if (!ok) return;

    this.studentService.deleteStudent(id).subscribe({
      next: () => {
        this.students.update(list => list.filter(s => s.id !== id));
      }
    });
  }

  onSearchChange(value: string) {
    this.searchQuery = value;
  }

  clearSearch() {
    this.searchQuery = '';
  }

  getFilteredStudents(): Profile[] {
    if (!this.searchQuery.trim()) {
      return this.students();
    }

    const query = this.searchQuery.toLowerCase().trim();
    return this.students().filter(student => {
      const fullName = `${student.lastName} ${student.firstName}`.toLowerCase();
      const phone = student.phoneNumber!.toLowerCase();
      return fullName.includes(query) || phone.includes(query);
    });
  }
}
