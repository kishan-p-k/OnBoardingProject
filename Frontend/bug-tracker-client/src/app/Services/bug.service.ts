import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
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
  deleteBug(ref_id: string): Observable<any> {
    console.log("Reference id ", {ref_id});
    return this.http.delete(`http://localhost:5135/bug/${ref_id}`);
  }
  updateBugField(
    ref_id: string,
    bugField: string,
    bugValue: string
  ): Observable<Bug> {

    const body = {
      bugField: bugField,
      bugValue: bugValue
    };

    return this.http.put<Bug>(
      `http://localhost:5135/bug/${ref_id}`,
      body
    );

  }


  createBug() {

  }
  }
