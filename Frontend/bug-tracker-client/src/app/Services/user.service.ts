import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  username: string;
  reference_id: string;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5135/users';

  searchUsers(value: string): Observable<string[]> {
    console.log("Value: ",value)
    return this.http.get<string[]>(`${this.baseUrl}/${value}`);
  }
}
