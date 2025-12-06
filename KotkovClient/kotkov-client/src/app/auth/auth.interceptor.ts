import { HttpInterceptorFn } from "@angular/common/http";
import { AuthService } from "./auth.service";
import { inject } from "@angular/core";
import { catchError } from "rxjs";

export const authTokenInterceptor: HttpInterceptorFn = (req, next) =>{
    const authService = inject(AuthService);
    const token = authService.token;

    if (!token) {
        return next(req);
    }

    req = req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    })

    return next(req)
    .pipe(
        catchError(error => {
            if (error.status === 401) {
                authService.logout();
            }
            throw error;
        })
    )
}