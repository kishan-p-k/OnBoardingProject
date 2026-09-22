import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  reference_id: string;
  username: string;
  mail: string;
}

@Injectable({
  providedIn: 'root'
})
export class LoginPageService {
  private readonly apiUrl = 'http://localhost:5135/api/userauth';
  private currentUser: User | null = null;
  constructor(private readonly http: HttpClient) { }

  setCurrentUser(user: User | null): void {
    this.currentUser = user;
    if (user)
    {
      sessionStorage.setItem('currentUser', JSON.stringify(user));
    }
  }
  getCurrentUser(): User | null {
    if (this.currentUser) {
      return this.currentUser;
    }
    const storedUser = sessionStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUser = JSON.parse(storedUser);
      return this.currentUser;
    }
    return null;
  }

  login(usernameOrMail: string, password: string): Observable<User | null>
  {
    return this.http.get<User | null>(`${this.apiUrl}/${usernameOrMail}/${password}`);
  }

  register(username: string, mail: string, password: string): Observable<User | null>
  {
    return this.http.post<User | null>(`${this.apiUrl}/register`, {
      username,
      mail,
      password
    });
  }
  logout() {
    sessionStorage.removeItem('currentUser');
    this.currentUser = null;
  }
}
