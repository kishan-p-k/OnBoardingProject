import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
export interface Comment {
  reference_id: string;
  comment: string;
  author: string;
  date: Date;
}

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private readonly http = inject(HttpClient);

  getCommentById(ref_id: string): Observable<Comment[]> {
    return this.http
      .get<Comment[]>(`http://localhost:5135/comment/${ref_id}`)
      .pipe(
        tap((comment: Comment[]) => {
          console.log('GetBug By ref_id response:', comment);
        })
      );
  }
}
