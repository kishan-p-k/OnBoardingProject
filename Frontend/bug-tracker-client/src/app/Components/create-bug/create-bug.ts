import { Component, signal, inject, ElementRef, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormControl,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom, of, switchMap, debounceTime, distinctUntilChanged } from 'rxjs';
import { UserBugsService, CreateBugRequest } from '../../Services/user-bugs-service';
import { LoginPageService } from '../../Services/login-page-service';
import { UserService } from '../../Services/user.service';

@Component({
  selector: 'create-bug',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-bug.html',
  styleUrls: ['./create-bug.css']
})
export class CreateBugComponent {
  private readonly fb = inject(FormBuilder);
  private readonly userBugsService = inject(UserBugsService);
  private readonly router = inject(Router);
  private readonly loginService = inject(LoginPageService);
  private readonly userService = inject(UserService);
  private readonly elementRef = inject(ElementRef);
  private user = this.loginService.getCurrentUser();

  bugForm: FormGroup;
  errorMessage = signal('');
  submitting = signal(false);

  assigneeControl = new FormControl('', { nonNullable: true });

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

  constructor() {
    this.bugForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      priority: ['Medium', Validators.required],
      assignee: ['', Validators.required]
    });
  }

  get title() { return this.bugForm.get('title'); }
  get description() { return this.bugForm.get('description'); }
  get priority() { return this.bugForm.get('priority'); }
  get assignee() { return this.bugForm.get('assignee'); }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    if (!this.showAssigneeDropdown) {
      return;
    }

    const clickedElement = event.target as Node;
    const componentElement = this.elementRef.nativeElement as HTMLElement;

    const assigneeSearch = componentElement.querySelector('.assignee-search');

    if (assigneeSearch && assigneeSearch.contains(clickedElement)) {
      return;
    }

    this.showAssigneeDropdown = false;
  }



  selectAssignee(user: string): void {
    this.bugForm.patchValue({ assignee: user });
    this.assigneeControl.setValue(user, { emitEvent: false });
    this.showAssigneeDropdown = false;
  }

  clearAssignee(): void {
    this.bugForm.patchValue({ assignee: '' });
    this.assigneeControl.setValue('');
  }


  async onSubmit(): Promise<void> {
    if (this.bugForm.invalid) {
      this.bugForm.markAllAsTouched();
      return;
    }

    const currentUser = this.loginService.getCurrentUser();
    if (!currentUser) {
      this.errorMessage.set('You must be logged in to create a bug.');
      return;
    }

    this.submitting.set(true);
    this.errorMessage.set('');

    try {
      const bugRequest: CreateBugRequest = {
        ...this.bugForm.value,
        createdBy: currentUser.username
      };

      const bug = await firstValueFrom(this.userBugsService.createBug(bugRequest));
      this.router.navigate(['/bug', bug.reference_id], { replaceUrl: true });
    } catch (err) {
      this.errorMessage.set('Failed to create bug. Please try again.');
      console.error(err);
    } finally {
      this.submitting.set(false);
    }
  }

  onCancel(): void {
    this.router.navigate(['/userbugs', this.user?.reference_id]);
  }
}
1
