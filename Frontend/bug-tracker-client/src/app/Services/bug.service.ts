import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
export interface Bug {
  bugId: number;
  title: string;
  description: string;
  priority: string;
  status: string;
  createdBy: number;
  assignee: number;
  createdDate: Date;
}

@Injectable({
  providedIn: 'root',
})
export class BugService {
  private readonly http = inject(HttpClient);

  getById(ref_id: string): Observable<Bug> {
    return this.http
      .get<Bug>(`http://localhost:5135/bug/${ref_id}`)
      .pipe(
        tap((bug: Bug) => {
          console.log('GetBug By ref_id response:', bug);
        })
      );
  }
}
