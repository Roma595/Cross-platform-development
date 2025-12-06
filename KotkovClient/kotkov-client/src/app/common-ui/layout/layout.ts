import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLinkWithHref, RouterLink} from "@angular/router";
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLinkWithHref, RouterLink],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {
    cookieService = inject(CookieService);
    onSearch(query: string) {
      console.log('search:', query.trim());
      // дальше можешь фильтровать сигнал/массив студентов
    }
}
