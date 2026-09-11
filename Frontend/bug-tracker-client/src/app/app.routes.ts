import { Routes } from '@angular/router';
import { LoginPage } from './Components/login-page/login-page';
import { BugListComponent } from './Components/bug-list/bug-list';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginPage },
  { path: 'bugs', component: BugListComponent },
  { path: '**', redirectTo: '/login' }
];
