import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable, switchMap } from 'rxjs';  
import { UserBugsService, Bug } from '../../Services/user-bugs-service';
import { AsyncPipe } from '@angular/common';
import { RouterLink,ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-bugs',
  standalone: true,
  imports: [AsyncPipe, RouterLink, CommonModule],
  templateUrl: './user-bugs.html',
  styleUrls: ['./user-bugs.css']
})
export class UserBugsComponent {
  bugs$: Observable<Bug[]>;
  errorMessage = '';

  constructor(private readonly userBugsService: UserBugsService, private readonly route: ActivatedRoute) {
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
}

