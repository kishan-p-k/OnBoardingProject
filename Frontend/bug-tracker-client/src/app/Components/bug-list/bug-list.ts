import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { BugListService, Bug } from '../../Services/bug-list-service';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-bug-list',
  standalone: true,
  imports: [AsyncPipe, RouterLink, CommonModule],
  templateUrl: './bug-list.html',
  styleUrls: ['./bug-list.css']
})
export class BugListComponent {
  bugs$: Observable<Bug[]>;
  errorMessage = '';

  constructor(private readonly bugListService: BugListService) {
    this.bugs$ = this.bugListService.getBugs();
    this.errorMessage = this.bugListService.errorMessage;
  }
}

