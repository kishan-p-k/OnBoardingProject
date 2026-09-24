import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { LoginPageService } from '../../Services/login-page-service';
@Component({
  imports: [],
  selector: 'app-navigation',
  standalone: true,
  styleUrls: ['./navigation.css'],
  templateUrl: './navigation.html',
})
export class Navigation {
  private router = inject(Router);
  private location = inject(Location);
  private loginService = inject(LoginPageService);
  private user = this.loginService.getCurrentUser();
  goToDashboard() {
    this.router.navigate(['/userbugs', this.user?.reference_id]);
  }

  goBack() {
    this.location.back();
  }
  profile() {
    this.router.navigate(['/profile', this.user?.reference_id]);
  }

  logout() {
    this.loginService.logout();
    this.router.navigate(['/login']);
  }
}
