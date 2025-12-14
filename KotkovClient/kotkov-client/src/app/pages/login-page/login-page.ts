import { Component, inject } from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.scss',
})
export class LoginPage {
    authService = inject(AuthService);
    router = inject(Router);
    loginForm = new FormGroup({
        login: new FormControl(null, Validators.required),
        password: new FormControl(null),
    });

    onSubmit() {
        if (this.loginForm.valid) {
            console.log(this.loginForm.value);
            //@ts-ignore
            this.authService.login(this.loginForm.value).subscribe({
                next: (data) =>{
                    this.router.navigate(['/courses']);
                    console.log(data);
                }
            });
        }
        

        
    }
}
