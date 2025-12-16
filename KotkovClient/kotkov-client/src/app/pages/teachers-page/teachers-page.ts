import { Component, inject, signal } from '@angular/core';
import { TeacherService } from '../../data/services/teacher.service';
import { Profile } from '../../data/interfaces/profile.interface';
import { ProfileCard } from '../../common-ui/profile-card/profile-card';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../data/services/course.service';
import { Account } from '../../auth/auth.interface';
import { AuthService } from '../../auth/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { Course } from '../../data/interfaces/course.interface';


@Component({
  selector: 'app-teachers-page',
  imports: [ProfileCard, FormsModule],
  templateUrl: './teachers-page.html',
  styleUrl: './teachers-page.scss',
})
export class TeachersPage {
  teacherService = inject(TeacherService);
  cookieService = inject(CookieService);
  courseService = inject(CourseService);
  authService = inject(AuthService);

  teachers = signal<Profile[]>([]);
  isCreateOpen = false;
  editingTeacher = signal<Profile | null>(null);
  account = signal<Account | null>(null);

  allcourses = signal<Course[]>([]);

  // для поиска
  searchQuery = '';


  ngOnInit(){
    this.loadTeachers();
  }

  loadTeachers(){
    this.teacherService.getAllTeachers().subscribe({
      next: (data: Profile[]) => {
        this.teachers.set(data);
        this.loadCourses();
      },
      error: () =>{
        console.log('error get all teachers');
      }
    });
  }

  loadCourses(){
    this.courseService.getAllCourses().subscribe({
      next: courses =>{
        this.allcourses.set(courses);
        this.attachCoursesToTeachers();
      },
      error: () =>{
        console.log('error get all courses');
      }
    });
  }

  attachCoursesToTeachers() {
    const teachers = this.teachers();
    const courses = this.allcourses();

    const updated = teachers.map(t => {
      const teacherCourses = courses
        .filter(c => c.teacherId === t.id)
        .map(c => ({ name: c.name })); // или твой тип CourseShort

      return {
        ...t,
        courses: teacherCourses
      };
    });

    this.teachers.set(updated);
  }


  openCreateDialog() {
    this.editingTeacher.set(null);
    this.isCreateOpen = true;
  }

  openEditDialog(teacher: Profile) {
    this.editingTeacher.set(teacher);
    this.isCreateOpen = true;
  }

  closeCreateDialog() {
    this.isCreateOpen = false;
    this.editingTeacher.set(null);
  }

  registerTeacher(account: Account){
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
    const editing = this.editingTeacher();

    if (!editing) {
      const newTeacher: Profile = {
        id: 0,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phoneNumber: formValue.phone,
        role: "Teacher",
        courses: []
      };

      this.teacherService.createTeacher(newTeacher).subscribe({
        next: teacher => {

          console.log('teacher from API', teacher);

          this.teachers.update(list => [...list, teacher]);
          this.closeCreateDialog();

          const acc: Account = {
            login: formValue.login,
            password: formValue.password,
            role: 'Teacher',
            userId: teacher.id
          };
          console.log(acc);
          this.registerTeacher(acc);
        },
        error: () =>{
          console.log('error create teacher');
        }
      });

      


    } else {
      const updated: Profile = {
        id: editing.id,
        firstName: formValue.firstName,
        lastName: formValue.lastName,
        phoneNumber: formValue.phone,
        role: 'Teacher',
        courses: editing.courses
      };

      this.teacherService.updateTeacher(updated.id, updated).subscribe({
        next: () => {
          this.loadTeachers();
          this.closeCreateDialog();
        },
        error: () =>{
          console.log('error update teacher');
        }
      });
    }
  }


  deleteTeacher(id: number) {
    const ok = window.confirm('Вы точно хотите удалить преподавателя?');
    if (!ok) return;

    this.teacherService.deleteTeacher(id).subscribe({
      next: () => {
        this.teachers.update(list => list.filter(t => t.id !== id));
      },
      error:() =>{
        alert("Преподаватель связан с курсом, нельзя удалить");
      }
    });
  }


  onSearchChange(value: string) {
    this.searchQuery = value;
  }


  clearSearch() {
    this.searchQuery = '';
  }


  getFilteredTeachers(): Profile[] {
    if (!this.searchQuery.trim()) {
      return this.teachers();
    }


    const query = this.searchQuery.toLowerCase().trim();
    return this.teachers().filter(teacher => {
      const fullName = `${teacher.lastName} ${teacher.firstName}`.toLowerCase();
      const phone = teacher.phoneNumber!.toLowerCase();
      return fullName.includes(query) || phone.includes(query);
    });
  }
}
