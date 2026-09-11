import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { BugListService, Bug } from '../../Services/bug-list-service';

@Component({
  selector: 'app-bug-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bug-list.html',
  styleUrl: './bug-list.css'
})
export class BugListComponent {
  bugs$: Observable<Bug[]>;
  errorMessage = '';

  constructor(private readonly bugListService: BugListService) {
    this.bugs$ = this.bugListService.getBugs();
    this.errorMessage = this.bugListService.errorMessage;
  }
}

