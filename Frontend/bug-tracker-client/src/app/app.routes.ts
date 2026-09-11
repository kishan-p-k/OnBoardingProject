import { Routes } from '@angular/router';
import { LoginPage } from './Components/login-page/login-page';
import { BugListComponent } from './Components/bug-list/bug-list';
import { BugComponent } from './Components/bug-component/bug-component';


export const routes: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'bug/:ref_id', component: BugComponent },
  { path: 'bug', component: BugListComponent },
];
