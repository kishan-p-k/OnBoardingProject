import { Component, inject } from '@angular/core';
import { Navigation } from '../navigation/navigation'; 
import { LoginPageService } from '../../Services/login-page-service'; 
@Component({
  imports: [Navigation],
  selector: 'app-profile',
  styleUrls: ['./profile.css'],
  templateUrl: './profile.html',
})
export class Profile {
  private readonly loginService = inject(LoginPageService);
  user = this.loginService.getCurrentUser();

}
