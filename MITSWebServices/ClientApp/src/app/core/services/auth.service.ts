import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() {}

  public getJwt() {
    const jwt: string = localStorage.getItem('MITSToken') || '';
    return jwt;
  }

  public setJwt(jwt: string) {
    localStorage.setItem('jwt', jwt);
  }
  
  public clear() {
    localStorage.removeItem('jwt');
  }
}
