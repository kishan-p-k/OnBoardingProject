import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LoginPageService } from '../../Services/login-page-service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  imports: [ReactiveFormsModule],
  standalone: true,
  selector: 'app-create-user',
  styleUrls: ['./create-user.css'],
  templateUrl: './create-user.html',
})
export class CreateUser {
  username = new FormControl('', [
    Validators.required,
  ]);
  email = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);
  newPassword = new FormControl('', [
    Validators.required,
    Validators.minLength(8)
  ]);
  confirmPassword = new FormControl('', [
    Validators.required,
    Validators.minLength(8)
  ]);
  role = new FormControl('', [
    Validators.required
  ]);
  registerForm = new FormGroup(
    {
    username: this.username,
    email: this.email,
    newPassword: this.newPassword,
    confirmPassword: this.confirmPassword,
    role: this.role
    },
    {
      validators: this.passwordMatchValidator
    }
  );

  errorMessage = '';

  constructor(
    private readonly loginService: LoginPageService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) { }

  passwordMatchValidator(
    form: AbstractControl
  ): ValidationErrors | null {

    const password = form.get('newPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  register() {
    if (this.registerForm.invalid) return;

    const username = this.username.value ?? '';
    const email = this.email.value ?? '';
    const password = this.newPassword.value ?? '';
    const role = this.role.value ?? '';

    this.loginService.register(username, email, password, role).pipe(
      tap((user) => {
        this.router.navigate(['/admin']);
      }),
      catchError((error) => {
        this.errorMessage = error.error;
        this.cdr.detectChanges();
        return of(null);
      })
    ).subscribe();
  }
}
