import { Component } from '@angular/core';
import {
  FormControl, FormGroup, ReactiveFormsModule, Validators, AbstractControl,ValidationErrors, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, shareReplay, tap } from 'rxjs/operators';
import { LoginPageService, User } from '../../Services/login-page-service';


@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-login-page',
  standalone: true,
  styleUrls: ['./login-page.css'],
  templateUrl: './login-page.html',
})

export class LoginPage {
  email = new FormControl('', [
    Validators.required,
    Validators.email,
    this.publicEmailValidator
  ]);
  password = new FormControl('', [
    Validators.required,
    Validators.minLength(8)
  ]);
  loginForm = new FormGroup({
    email: this.email,
    password: this.password,
  });


  user$: Observable<User | null> | null = null;
  errorMessage = '';

  constructor(private readonly loginService: LoginPageService, private readonly router: Router) { }

  publicEmailValidator(control: AbstractControl): ValidationErrors | null {
    const email = control.value;

    if (!email) {
      return null;
    }

    const domain = email.split('@')[1];

    if (!domain || !domain.includes('.') || !/\.[a-zA-Z]{3,}$/.test(domain)) {
      return {
        invalidPublicDomain: true
      };
    }

    return null;
  }

  login() {
    if (this.loginForm.invalid) return;

    this.errorMessage = '';
    const email = this.email.value ?? '';
    const password = this.password.value ?? '';

    this.user$ = this.loginService.login(email, password).pipe(
      tap((user) => {
        console.log('Login successful:', user);

        this.loginService.setCurrentUser(user);

        this.router.navigate(['/userbugs', user?.reference_id]);
      }),
      
      catchError((error) => {
        console.error('Login error:', error);
        this.errorMessage = 'Failed to login. Please check your credentials.';
        return of(null);
      }),
      shareReplay(1)
    );
    this.user$.subscribe();

  }
  goToSignup(): void {
    this.router.navigate(['/signup']);
  }
}
