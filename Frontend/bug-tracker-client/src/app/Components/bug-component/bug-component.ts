import { AsyncPipe, DatePipe } from '@angular/common';
import {
  Component,
  inject,
  HostListener,
  ElementRef
} from '@angular/core';

import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import {
  merge,
  Subject,
  of,
  switchMap,
  debounceTime,
  distinctUntilChanged
} from 'rxjs';

import { BugService } from '../../Services/bug.service';
import { UserService } from '../../Services/user.service';

import {
  FormsModule,
  ReactiveFormsModule,
  FormControl
} from '@angular/forms';

import { CommentComponent } from '../comment-component/comment-component';
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
    ReactiveFormsModule,
    Navigation
  ],

  templateUrl: './bug-component.html',
  styleUrl: './bug-component.css',
})
export class BugComponent {

  private readonly route = inject(ActivatedRoute);
  private readonly bugService = inject(BugService);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);
  private readonly elementRef = inject(ElementRef);


  // =========================
  // ASSIGNEE SEARCH
  // =========================

  assigneeControl = new FormControl('', {
    nonNullable: true
  });

  showAssigneeDropdown = false;


  assigneeValue$ = this.assigneeControl.valueChanges.pipe(

    debounceTime(300),

    distinctUntilChanged(),

    switchMap(value =>
      value.trim().length > 0
        ? this.userService.searchUsers(value)
        : of([])
    )

  );


  // =========================
  // EDITING
  // =========================

  editingField: string | null = '';

  editedValue = '';


  // =========================
  // UPDATED BUG
  // =========================

  private readonly updatedBug$ = new Subject<Bug>();


  // =========================
  // DELETE BUG
  // =========================

  deleteBug(ref_id: string): void {

    const confirmed = window.confirm(
      'Are you sure you want to delete this bug?'
    );

    if (!confirmed) {
      return;
    }

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



  fieldChanged(
    event: Event,
    ref_id: string,
    bugField: string
  ): void {

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

        this.showAssigneeDropdown = false;

      },

      error: error => {

        console.error(error);

        window.alert('Failed to update bug.');

      }

    });

  }


  // =========================
  // OUTSIDE CLICK
  // =========================

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {

    if (!this.showAssigneeDropdown) {
      return;
    }

    const clickedElement = event.target as Node;

    const componentElement =
      this.elementRef.nativeElement as HTMLElement;


    // If the click happened anywhere inside
    // the assignee search area, do nothing.
    const assigneeSearch =
      componentElement.querySelector('.assignee-search');


    if (
      assigneeSearch &&
      assigneeSearch.contains(clickedElement)
    ) {
      return;
    }


    // Otherwise hide the dropdown.
    this.showAssigneeDropdown = false;

  }


  // =========================
  // START EDITING
  // =========================

  startEditing(field: string, value: string): void {

    this.editingField = field;

    this.editedValue = value;


    if (field === 'assignee') {

      this.assigneeControl.setValue('');

      this.showAssigneeDropdown = true;

    }

  }


  // =========================
  // SELECT ASSIGNEE
  // =========================

  selectAssignee(
    user: string,
    ref_id: string
  ): void {

    this.showAssigneeDropdown = false;


    const event = {
      target: {
        value: user
      }
    } as unknown as Event;


    this.fieldChanged(
      event,
      ref_id,
      'assignee'
    );

  }


  // =========================
  // SAVE FIELD
  // =========================

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


    this.fieldChanged(
      event,
      ref_id,
      field
    );

  }


  // =========================
  // CANCEL EDIT
  // =========================

  cancelEdit(): void {

    this.editingField = null;

    this.editedValue = '';

    this.showAssigneeDropdown = false;

  }


  // =========================
  // GET BUG
  // =========================

  bug$ = merge(

    this.route.paramMap.pipe(

      switchMap(params => {

        const ref_id = params.get('ref_id');

        if (!ref_id) {
          throw new Error(
            'Ref ID is missing from the route'
          );
        }

        return this.bugService.getById(ref_id);

      })

    ),

    this.updatedBug$

  );

}
