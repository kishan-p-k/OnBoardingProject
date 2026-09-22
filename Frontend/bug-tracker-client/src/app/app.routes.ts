import { Routes } from '@angular/router';
import { LoginPage } from './Components/login-page/login-page';
import { BugListComponent } from './Components/bug-list/bug-list';
import { BugComponent } from './Components/bug-component/bug-component';
import { UserBugsComponent } from './Components/user-bugs/user-bugs';
import { CreateBugComponent } from './Components/create-bug/create-bug';
import { authGuard } from './auth-guard'
import { CreateUser } from './Components/create-user/create-user';


export const routes: Routes = [
  { path: 'login', component: LoginPage },

  { path: 'signup', component: CreateUser },

  { path: 'bug/create', component: CreateBugComponent, canActivate: [authGuard] },

  { path: 'bug/:ref_id', component: BugComponent, canActivate: [authGuard] },

  { path: 'bug', component: BugListComponent, canActivate: [authGuard] },

  { path: 'userbugs/:ref_id', component: UserBugsComponent, canActivate: [authGuard] },

  { path: '**', redirectTo: '/login', pathMatch: 'full' }

];
