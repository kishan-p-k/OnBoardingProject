import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, switchMap } from 'rxjs';  
import { UserBugsService, Bug } from '../../Services/user-bugs-service';
import { AsyncPipe } from '@angular/common';
import { RouterLink,ActivatedRoute,Router } from '@angular/router';
import { LoginPageService } from '../../Services/login-page-service';
import { User } from '../../Services/user.service';
import { Navigation } from '../navigation/navigation';

@Component({
  selector: 'app-user-bugs',
  standalone: true,
  imports: [AsyncPipe, RouterLink, CommonModule, Navigation],
  templateUrl: './user-bugs.html',
  styleUrls: ['./user-bugs.css']
})
export class UserBugsComponent implements OnInit {
  bugs$: Observable<Bug[]>;
  errorMessage = '';
  user: User | null = null;

  ViewAllBugs()
  {
    console.log("View All Bugs clicked");
    this.router.navigate(['/bug']);
  }
  constructor(private readonly userBugsService: UserBugsService, private readonly loginService: LoginPageService, private readonly route: ActivatedRoute, private readonly router: Router) {
    this.bugs$ = this.route.paramMap.pipe(
      switchMap(params => {
        const ref_id = params.get('ref_id');

        if (!ref_id) {
          throw new Error('Ref ID is missing from the route');
        }

        return this.userBugsService.GetUserBugs(ref_id);
      })
    );
    this.errorMessage = this.userBugsService.errorMessage;
  }
  ngOnInit(): void {
    this.user = this.loginService.getCurrentUser();

    console.log(this.user?.username);
  }
  AddBugs() {
    console.log("New Bug Button clicked");
     //this.router.navigate(['/bug/create']);
  }
}

