import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

export interface BugFilter {
  status?: string;
  priority?: string;
  assignee?: string;
  createdBy?: string;
}

@Component({
  imports: [FormsModule],
  standalone: true,
  selector: 'app-filter-box',
  styleUrls: ['./filter-box.css'],
  templateUrl: './filter-box.html',
})
export class FilterBox {
  @Output() filterChanged = new EventEmitter<BugFilter>();

  filter: BugFilter = {
    status: '',
    priority: '',
    assignee: '',
    createdBy: ''
  };

  applyFilter() {
    this.filterChanged.emit(this.filter);
  }

  clearFilter() {
    this.filter = {
      status: '',
      priority: '',
      assignee: '',
      createdBy: ''
    };
    this.applyFilter();
  }
}
