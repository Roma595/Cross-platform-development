import { Component, inject, signal } from '@angular/core';
import { StudentService } from '../../data/services/student.service';
import { Profile } from '../../data/interfaces/profile.interface';
import { Course } from '../../data/interfaces/course.interface';
import { ProfileCard } from '../../common-ui/profile-card/profile-card';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../data/services/course.service';
import { catchError, forkJoin, map, of } from 'rxjs';
import { Account } from '../../auth/auth.interface';
import { AuthService } from '../../auth/auth.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-students-page',
  imports: [ProfileCard, FormsModule],
  templateUrl: './students-page.html',
  styleUrl: './students-page.scss',
})
export class StudentsPage {
  studentService = inject(StudentService);
  cookieService = inject(CookieService);
  courseService = inject(CourseService);
  authService = inject(AuthService);

  students = signal<Profile[]>([]);
  courses = signal<Course[]>([]);

  isCreateOpen = false;
  isDetailOpen = false;

  editingStudent = signal<Profile | null>(null);
  selectedStudent = signal<Profile | null>(null);
  selectedCourseId = signal<number | null>(null);
  countTakenPlaces = signal<number>(0);
  takenPlacesByCourse = new Map<number, number>();

  account = signal<Account | null>(null);
  
  // для поиска
  searchQuery = '';

  ngOnInit(){
    this.loadStudents();
    this.loadCourses();
  }

  loadStudents(){
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
      error: () =>{
        console.log('error get all students');
      }
    });
  }

  loadCourses(){
    this.courseService.getAllCourses().subscribe({
      next: courses =>{
        this.courses.set(courses);
        this.loadTakenPlacesForAllCourses();
      },
      error: () =>{
        console.log('error get all courses');
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

  registerStudent(account: Account){
    this.authService.register(account).subscribe({
      next: () =>{
        console.log("register ok");
      },
      error: () =>{
        console.log("register failed");
      }      
    });
  }

  submitCreateOrEdit(formValue: any) {
    const editing = this.editingStudent();

    if (!editing) {
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
          const acc: Account = {
            login: formValue.login,
            password: formValue.password,
            role: 'Student',
            userId: student.id
          };
          this.registerStudent(acc);
        },
        error: () =>{
          console.log('error create student');
        }
      });

      
    } else {
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
        error: () =>{
          console.log('error update student');
        }
      });
    }
  }

  openDetailDialog(student: Profile) {
    this.selectedStudent.set(student);
    console.log(student);
    this.selectedCourseId.set(null);
    this.isDetailOpen = true;
  }

  closeDetailDialog() {
    this.isDetailOpen = false;
    this.selectedStudent.set(null);
    this.selectedCourseId.set(null);
  }

  loadTakenPlacesForAllCourses() {
    const courses = this.courses();
    if (!courses.length) return;
    const requests = courses.map(c =>
      this.studentService.getAllStudentsByCourseId(c.id).pipe(
        map(students => ({ courseId: c.id, count: students.length })),
        catchError(() => of({ courseId: c.id, count: 0 }))
      )
    );

    forkJoin(requests).subscribe(results => {
      this.takenPlacesByCourse = new Map(
        results.map(r => [r.courseId, r.count])
      );
    });

  }

  getAvailableCoursesForSelected() {
    const today = new Date();
    const selected = this.selectedStudent();
    if (!selected) return [];

    const enrolledNames = new Set((selected.courses ?? []).map(c => c.name));

    return this.courses().filter(course => {
      const end = new Date(course.endDate);
      const isFuture = end > today;

      const notEnrolled = !enrolledNames.has(course.name);

      const taken = this.takenPlacesByCourse.get(course.id) ?? 0;
      const hasFreePlaces = taken < course.totalPlaces;

      return isFuture && notEnrolled && hasFreePlaces;
    });
  }

  addToCourse() {
    const student = this.selectedStudent();
    const courseId = this.selectedCourseId();
    
    if (!student || !courseId) {
      alert('Выберите курс для зачисления');
      return;
    }

    this.studentService.addToCourse(student.id, courseId!, 1).subscribe({
      next: () => {
        const course = this.courses().find(c => c.id === courseId);
        if (course && this.selectedStudent()) {
          this.selectedStudent()!.courses = [
            ...(this.selectedStudent()!.courses || []),
            { name: course.name }
          ];
          
          this.students.update(list =>
            list.map(s => s.id === student.id ? { ...this.selectedStudent()! } : s)
          );
        }
        this.selectedCourseId.set(null);
        alert('Студент успешно зачислен на курс!');
      },
      error: (err) => {
        alert('Ошибка при зачислении на курс');
        console.error(err);
      }
    });
  }

  deleteStudent(id: number) {
    const ok = window.confirm('Вы точно хотите удалить студента?');
    if (!ok) return;

    this.studentService.deleteStudent(id).subscribe({
      next: () => {
        this.students.update(list => list.filter(s => s.id !== id));
      },
      error: () =>{
        console.log('error delete student');
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
