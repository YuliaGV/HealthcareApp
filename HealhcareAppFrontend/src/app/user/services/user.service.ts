
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Login } from '../interfaces/login';
import { Observable } from 'rxjs';
import { Session } from '../interfaces/session';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl: string = environment.apiURL  + "User/"

  constructor(private http: HttpClient) { }

  signInService(request: Login): Observable<Session>{
    return this.http.post<Session>(`${this.baseUrl}login`, request)
  }

}
