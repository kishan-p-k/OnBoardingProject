import { Component, EventEmitter, Output, Input, OnChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  switchMap,
  of
} from 'rxjs';
import { UserService } from '../../Services/user.service';
export interface BugFilter {
  reference_id?: string | null;
  keyword?: string;
  status?: string;
  priority?: string;
  assignee?: string;
  createdBy?: string;
}

@Component({
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  standalone: true,
  selector: 'app-filter-box',
  styleUrls: ['./filter-box.css'],
  templateUrl: './filter-box.html',
})
export class FilterBox implements OnChanges {
  private readonly userService = inject(UserService);

  @Input() reference_id: string | null = null;
  @Input() resetFilter = false;

  @Output() filterChanged = new EventEmitter<BugFilter>();

  keywordControl = new FormControl('');
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

  selectAssignee(user: string): void {
    this.assigneeControl.setValue(user);
    this.showAssigneeDropdown = false;
    this.filter.assignee = user;
    this.applyFilter();
  }

  filter: BugFilter = {
    reference_id: '',
    keyword: '',
    status: '',
    priority: '',
    assignee: '',
    createdBy: ''
  };

  constructor() {
    this.keywordControl.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(keyword => {
        this.filter.keyword = keyword ?? '';
        this.applyFilter();
      });
  }

  ngOnChanges(): void {
    if (this.resetFilter) {
      this.clearFilter();
    }
  }

  highlightKeyword(title: string, keyword: string): string {
    if (!keyword) {
      return title;
    }

    const escapedKeyword = keyword.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

    const regex = new RegExp(`(${escapedKeyword})`, 'gi');

    return title.replace(regex, '<mark>$1</mark>');
  }

  applyFilter(): void {
    this.filter.reference_id = this.reference_id;
    this.filterChanged.emit(this.filter);
  }

  clearFilter(): void {
    this.keywordControl.setValue('', { emitEvent: false });

    this.filter = {
      status: '',
      priority: '',
      assignee: '',
      createdBy: '',
      reference_id: ''
    };

    this.applyFilter();
  }
  clearSearch(): void {
    this.assigneeControl.setValue('', { emitEvent: false });
    this.showAssigneeDropdown = false;
  }
}
