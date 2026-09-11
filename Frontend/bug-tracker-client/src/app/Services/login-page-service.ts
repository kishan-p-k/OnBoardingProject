import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  userId: number;
  username: string;
  mail: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginPageService {
  private readonly apiUrl = 'http://localhost:5135/api/userauth';

  constructor(private readonly http: HttpClient) { }

  login(usernameOrMail: string, password: string): Observable<User | null> {
    return this.http.get<User | null>(`${this.apiUrl}/${usernameOrMail}/${password}`);
  }
}
