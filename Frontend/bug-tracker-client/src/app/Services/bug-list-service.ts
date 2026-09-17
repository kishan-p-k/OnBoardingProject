import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

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
}

