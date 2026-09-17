import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  username: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5135/users';

  search(query: string): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/search?query=${query}`);
  }
}
