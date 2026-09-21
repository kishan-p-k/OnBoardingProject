import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { BugFilter } from '../Components/filter-box/filter-box';
import { HttpParams } from '@angular/common/http';
export interface Bug {
  reference_id: string;
  title: string;
  description: string;
  priority: string;
  status: string;
  createdBy: number;
  assignee: number | null;
  createdDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class BugListService {
  bugs$: Observable<Bug[]>;
  errorMessage = '';

  private activeFilter: BugFilter = {};
  private readonly bugsApiUrl = 'http://localhost:5135/bugs';
  constructor(private readonly http: HttpClient) {
    this.bugs$ = this.http.get<Bug[]>(this.bugsApiUrl).pipe(
      tap((bugs) => {
        console.log('API response:', bugs);
      }),
      catchError((error) => {
        console.error('API error:', error);
        this.errorMessage = 'Failed to load bugs.';
        return of([]);
      })
    );
  }

  
  getBugs(): Observable<Bug[]> {
    return this.bugs$;
  }

  getFilteredBugs(): Observable<Bug[]> {
    if (Object.keys(this.activeFilter).length === 0) {
      return this.getBugs();
    }

    return this.filterBugs(this.activeFilter);
  }

  filterBugs(filter: BugFilter): Observable<Bug[]> {

    this.activeFilter = filter

    let params = new HttpParams();
    if (filter.keyword) {
      params = params.set('keyword', filter.keyword);
    }
    if (filter.status) {
      params = params.set('status', filter.status);
    }
    if (filter.priority) {
      params = params.set('priority', filter.priority);
    }
    if (filter.assignee) {
      params = params.set('assignee', filter.assignee);
    }
    if (filter.createdBy) {
      params = params.set('createdBy', filter.createdBy);
    }

    return this.http.get<Bug[]>(`${this.bugsApiUrl}/filter`, { params }).pipe(
      tap((bugs) => {
        console.log('API response:', bugs);
      }),
      catchError((error) => {
        console.error('API error:', error);
        this.errorMessage = 'Failed to load bugs.';
        return of([]);
      })
    );
  }
}

