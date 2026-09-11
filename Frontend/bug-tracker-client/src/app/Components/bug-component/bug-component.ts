import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { BugService } from '../../Services/bug.service';

export interface Bug {
  bugId: number;
  title: string;
  description: string;
  priority: string;
  status: string;
  createdBy: string;
  assignee: string;
  createdDate: Date;
}

@Component({
  selector: 'app-bug-component',
  standalone: true,
  imports: [AsyncPipe, RouterLink, DatePipe],
  templateUrl: './bug-component.html',
  styleUrl: './bug-component.css',
})
export class BugComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly bugService = inject(BugService);

  bug$ = this.route.paramMap.pipe(
    switchMap(params => {
      const ref_id = params.get('ref_id');

      if (!ref_id) {
        throw new Error('Ref ID is missing from the route');
      }

      return this.bugService.getById(ref_id);
    })
  );
}
