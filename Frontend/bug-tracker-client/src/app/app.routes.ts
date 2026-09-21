import { Routes } from '@angular/router';
import { LoginPage } from './Components/login-page/login-page';
import { BugListComponent } from './Components/bug-list/bug-list';
import { BugComponent } from './Components/bug-component/bug-component';
import { UserBugsComponent } from './Components/user-bugs/user-bugs';
import { CreateBugComponent } from './Components/create-bug/create-bug';


export const routes: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'bug/create', component: CreateBugComponent },

  { path: 'bug/:ref_id', component: BugComponent },
  { path: 'bug', component: BugListComponent },
  { path: 'userbugs/:ref_id', component: UserBugsComponent },
  { path: '**', redirectTo: '/login', pathMatch: 'full' }
];
