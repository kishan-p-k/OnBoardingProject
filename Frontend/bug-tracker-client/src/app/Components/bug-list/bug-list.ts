import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { BugListService, Bug } from '../../Services/bug-list-service';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BugFilter, FilterBox } from '../filter-box/filter-box';
import { Navigation } from '../navigation/navigation';


@Component({
  selector: 'app-bug-list',
  standalone: true,
  imports: [AsyncPipe, RouterLink, CommonModule, Navigation, FilterBox],
  templateUrl: './bug-list.html',
  styleUrls: ['./bug-list.css']
})
export class BugListComponent {
  //private bugListService = inject(BugListService);
  bugs$: Observable<Bug[]>;
  errorMessage = '';
  
  applyFilter(filter: BugFilter) {
    this.bugs$ = this.bugListService.filterBugs(filter);
    console.log(filter);
  }
  constructor(private readonly bugListService: BugListService) {
    this.bugs$ = this.bugListService.getFilteredBugs();
    this.errorMessage = this.bugListService.errorMessage;
  }
}

