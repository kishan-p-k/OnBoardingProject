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
  createdBy: string;
  assignee: number | null;
  createdDate: Date;
}
export interface CreateBugRequest {
  title: string;
  description: string;
  priority: string;
  assignee: string;
  status: string;
  created_by: string;
}
@Injectable({
  providedIn: 'root'
})
export class UserBugsService {
  //bugs$: Observable<Bug[]>;
  errorMessage = '';
  private readonly bugsApiUrl = 'http://localhost:5135/userbugs';

  constructor(private readonly http: HttpClient) { }
    GetUserBugs(reference_id: string){
      return this.http.get<Bug[]>(`${this.bugsApiUrl}/${reference_id}`).pipe(
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


  createBug(bug: CreateBugRequest): Observable<Bug> {
    console.log("Request recieved to insert{bug}",bug)
  return this.http.post<Bug>(`http://localhost:5135/userbugs/create`, bug).pipe(
    tap((created: Bug) => {
      console.log('Created bug:', created);
    })
  );
}
}


