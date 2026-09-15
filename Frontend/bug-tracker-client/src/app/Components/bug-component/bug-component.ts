import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { switchMap } from 'rxjs';
import { BugService } from '../../Services/bug.service';

export interface Bug {
  reference_id: string;
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
  private readonly router = inject(Router);


  deleteBug(ref_id: string): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete this bug?'
    );

    if (!confirmed) {
      return;
    }
    console.log("Component refid", { ref_id });
    this.bugService.deleteBug(ref_id).subscribe({
      next: () => {
        this.router.navigate(['/bug']);
      },
      error: error => {
        console.error(error);
        window.alert('Failed to delete bug.');
      }
    });
  }
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
