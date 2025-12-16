import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLinkWithHref, RouterLink} from "@angular/router";
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../../auth/auth.service';
import { single } from 'rxjs';
import { Profile } from '../../data/interfaces/profile.interface';
import { StudentsPage } from '../../pages/students-page/students-page';
import { StudentService } from '../../data/services/student.service';
import { TeacherService } from '../../data/services/teacher.service';


@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLinkWithHref, RouterLink],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {
    cookieService = inject(CookieService);
    authService = inject(AuthService);
    studentService = inject(StudentService);
    teacherService = inject(TeacherService);

    userMenuOpen = false;
    userRoleRus = signal<string | null>(null);
    userRole = signal<string | null>(null);
    userInitials = signal<string>("");

    user = signal<Profile | null>(null);

    ngOnInit(){
      this.userRole.set(this.cookieService.get('role'));
      switch(this.userRole()){
        case "Admin":
          this.userRoleRus.set("Администратор");
          break;
        case "Student":
          this.userRoleRus.set("Студент");
          break;
        case "Teacher":
          this.userRoleRus.set("Преподаватель");
          break;
      }

      this.getUserInitials();
    }

    toggleUserMenu() {
      this.userMenuOpen = !this.userMenuOpen;
    }

    getUserInitials(){
      const userId = parseInt(this.cookieService.get('user_id'));
      const role = this.cookieService.get('role');

      if(userId === 0){
        this.userInitials.set('');
      }

      if (role === 'Student'){
        this.studentService.getStudentById(userId).subscribe({
          next: student =>{
            this.user.set(student);
            if (this.user() !== null){
              this.userInitials.set(`${this.user()?.firstName[0]}${this.user()?.lastName[0]}`);
            }
          },
          error: ()=>{
            console.log("error load student");
          }
        });
      }

      else if(role === 'Teacher'){
        this.teacherService.getTeacherById(userId).subscribe({
          next: teacher =>{
            this.user.set(teacher);

            if (this.user() !== null){
              this.userInitials.set(`${this.user()?.firstName[0]}${this.user()?.lastName[0]}`);
            }
          },
          error: () =>{
            console.log("error load teacher");
          }
        });
      }

    }

    goToProfile() {
      
    }

    logout() {
      this.authService.logout();
      this.userMenuOpen = false;
    }
}
