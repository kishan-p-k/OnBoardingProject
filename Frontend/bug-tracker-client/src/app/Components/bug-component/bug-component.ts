import { AsyncPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { merge, Subject, switchMap } from 'rxjs';
import { BugService } from '../../Services/bug.service';
import { FormsModule } from '@angular/forms'
import { Navigation } from '../navigation/navigation';
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

  imports: [
    AsyncPipe,
    RouterLink,
    DatePipe,
    FormsModule,
    CommentComponent,
    ReactiveFormsModule
  ],

  templateUrl: './bug-component.html',
  styleUrl: './bug-component.css',
})
export class BugComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly bugService = inject(BugService);
  private readonly router = inject(Router);

  editingField: string | null = null;
  editedValue = '';

  private readonly updatedBug$ = new Subject<Bug>();

  deleteBug(ref_id: string): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete this bug?'
    );

    if (!confirmed) {
      return;
    }

    console.log('Component refid', { ref_id });

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

  fieldChanged(event: Event, ref_id: string, bugField: string): void {
    const bugValue = (
      event.target as HTMLInputElement | HTMLSelectElement
    ).value;

    this.bugService.updateBugField(
      ref_id,
      bugField,
      bugValue
    ).subscribe({
      next: updatedBug => {
        console.log('Updated bug:', updatedBug);
        this.updatedBug$.next(updatedBug);

        this.editingField = null;
        this.editedValue = '';
      },
      error: error => {
        console.error(error);
        window.alert('Failed to update bug.');
      }
    });
  }

  startEditing(field: string, value: string): void {
    this.editingField = field;
    this.editedValue = value;
  }

  saveField(ref_id: string): void {
    if (!this.editingField) {
      return;
    }

    const field = this.editingField;
    const value = this.editedValue;

    const event = {
      target: {
        value: value
      }
    } as unknown as Event;

    this.fieldChanged(event, ref_id, field);
  }

  cancelEdit(): void {
    this.editingField = null;
    this.editedValue = '';
  }

  bug$ = merge(
    this.route.paramMap.pipe(
      switchMap(params => {
        const ref_id = params.get('ref_id');

        if (!ref_id) {
          throw new Error('Ref ID is missing from the route');
        }

        return this.bugService.getById(ref_id);
      })
    ),
    this.updatedBug$
  );
}
