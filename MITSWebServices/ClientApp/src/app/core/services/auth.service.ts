import { Injectable } from '@angular/core';
import { JwtHelperService } from './jwt-helper.service';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from "rxjs/Operators";
import { Observable } from 'rxjs';

import { environment } from "../../../environments/environment";

interface ILoginResponse {
  access_token: string;
}

interface AccessToken {
  aud: string;
  exp: number;
  iat: number;
  iss: string;
  jti: string;
  nbf: number;
  role: string;
  sub: string;
  token_usage: string;

}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwt: JwtHelperService, private http: HttpClient) {}

  login(username: string, password: string): Observable<boolean> {

    const headers = new HttpHeaders()
      .set('Content-Type', 'application/x-www-form-urlencoded');
     const body = new HttpParams()
      .set('grant_type', 'password')
      .set('username', username)
      .set('password', password);

    return this.http.post<ILoginResponse>(environment.base_api + 'connect/token', body,

      { headers: headers }).pipe(map((loginResponse: ILoginResponse) => {
        let accessToken = loginResponse.access_token;
        if (accessToken) {
          console.log(accessToken);
          localStorage.setItem('MITSToken', accessToken);
          return true;
        } else {

          return false;
        }
      }));
  }

  tokenNotExpired(): boolean {
    const token = this.getJwt();

    return token !== null && !this.jwt.isTokenExpired(token);
  }

  isAdmin(): boolean {
    let jwt: string = this.getJwt();

    let token: AccessToken = this.jwt.decodeToken(jwt);
    
    if (token.role == 'Admin') {
      console.log('it is true');
      return true;
    }

    return false;
  }

  isCheckin(): boolean {
    let jwt: string = this.getJwt();

    let token: AccessToken = this.jwt.decodeToken(jwt);
    
    if (token.role == 'Checkin') {
      console.log('it is true');
      return true;
    }

    return false;
  }


  getJwt(): string {
    let jwt = localStorage.getItem('MITSToken') || '';
    return jwt;
  }

  setJwt(jwt: string) {

    localStorage.setItem('MITSToken', jwt);
  }
  
  clear() {
    localStorage.removeItem('MITSToken');
  }

  
}
