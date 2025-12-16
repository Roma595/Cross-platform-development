import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Account, TokenResponse } from './auth.interface';
import {CookieService} from 'ngx-cookie-service';
import { Router } from '@angular/router';
import {jwtDecode} from 'jwt-decode';

interface JwtPayload {
  [key: string]: any;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
    http = inject(HttpClient);
    router = inject(Router);
    cookieService = inject(CookieService);
    baseApiUrl = 'http://localhost:5158/api/';

    role_claim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

    role = signal<string | null>(null);
    user_id = signal<number>(0);
    payload_jwt: JwtPayload | null = null;
    token: string | null = null;

    get isAuth() {
        if (!this.token) {
            this.token = this.cookieService.get('token');
        }
        return !!this.token;
    }

    login(payload:{login: string; password: string}){
        return this.http.post<TokenResponse>(`${this.baseApiUrl}Account/token`, payload).pipe(
            tap(val =>{
                this.token = val.access_Token;
                this.cookieService.set('token', this.token);

                this.payload_jwt = jwtDecode<JwtPayload>(this.token!);
                this.role.set(this.payload_jwt[this.role_claim] as string | null);
                this.cookieService.set('role', this.role()? this.role()! : '') ;

                const rawUserId = this.payload_jwt['userId'] as string;

                const userId = typeof rawUserId === 'string' ? parseInt(rawUserId, 10) : rawUserId;
                
                this.user_id.set(userId);       
                this.cookieService.set('user_id', rawUserId? rawUserId : '');         
            })
        )
    }

    register(account: Account): Observable<any> {
        return this.http.post<Account>(`${this.baseApiUrl}Account/reg`, account);
    }

    logout(){
        this.cookieService.delete('token');
        this.cookieService.delete('role');
        this.role.set(null);
        this.user_id.set(0);
        this.token = null;
        this.router.navigate(['/login']);
    }
}
