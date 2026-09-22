import { Component, EventEmitter, Output, Input } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  merge, Subject, of, switchMap,
  debounceTime,
  distinctUntilChanged
} from 'rxjs';

export interface BugFilter {
  reference_id?: string|null;
  keyword?: string;
  status?: string;
  priority?: string;
  assignee?: string;
  createdBy?: string;
}

@Component({
  imports: [FormsModule,ReactiveFormsModule],
  standalone: true,
  selector: 'app-filter-box',
  styleUrls: ['./filter-box.css'],
  templateUrl: './filter-box.html',
})
export class FilterBox {
  @Input() reference_id: string | null = null;
  @Output() filterChanged = new EventEmitter<BugFilter>();
  keywordControl = new FormControl('');

  filter: BugFilter = {
    reference_id:'',
    keyword:'',
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
  highlightKeyword(title: string, keyword: string): string {
    if (!keyword) {
      return title;
    }

    const escapedKeyword = keyword.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

    const regex = new RegExp(`(${escapedKeyword})`, 'gi');

    return title.replace(regex, '<mark>$1</mark>');
  }

  applyFilter() {
    this.filter.reference_id = this.reference_id;
    this.filterChanged.emit(this.filter);
  }

  clearFilter() {
    this.keywordControl.setValue('', { emitEvent: false });
    this.filter = {
      status: '',
      priority: '',
      assignee: '',
      createdBy: '',
      reference_id:''
    };
    this.applyFilter();
  }
}
