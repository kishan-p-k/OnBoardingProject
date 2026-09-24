import { Component, inject } from '@angular/core';
import { Navigation } from '../navigation/navigation';
import { Router } from '@angular/router';
import { User, AdminService } from '../../Services/admin.service';
import { Observable } from 'rxjs';
import { AsyncPipe } from "@angular/common"; 


@Component({
  imports: [Navigation, AsyncPipe],
  selector: 'app-admin',
  styleUrl: './admin.css',
  templateUrl: './admin.html',
})
export class Admin {
  users!: Observable<User[]>;
  private readonly router = inject(Router);
  private readonly adminService = inject(AdminService);

  ngOnInit(): void {
    this.users = this.adminService.getAllUsers();
  }

  createNewUser(): void {
    this.router.navigate(['/signup']);
  }
}
