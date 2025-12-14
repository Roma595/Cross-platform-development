import { Component, inject, signal } from '@angular/core';
import { RouterOutlet, RouterLinkWithHref, RouterLink} from "@angular/router";
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../../auth/auth.service';
import { single } from 'rxjs';


@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLinkWithHref, RouterLink],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {
    cookieService = inject(CookieService);
    authService = inject(AuthService);

    userMenuOpen = false;
    userRoleRus = signal<string | null>(null);
    userRole = signal<string | null>(null);      // можно взять из AuthService
    userInitials = "";             // сгенерируй из ФИО

    ngOnInit(){
      this.userRole.set(this.authService.get_role());
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
    }
    toggleUserMenu() {
      this.userMenuOpen = !this.userMenuOpen;
    }

    goToProfile() {
      
    }

    logout() {
      this.authService.logout();
      this.userMenuOpen = false;
    }
}
