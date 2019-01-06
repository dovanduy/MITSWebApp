import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AdminDataService } from 'src/app/admin/services/admin-data.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private auth: AuthService, private data: AdminDataService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // if (!this.auth.loggedIn) {
    //   localStorage.setItem('authRedirect', state.url);
    // }
    // if (!this.auth.tokenValid && !this.auth.loggedIn) {
    //   this.auth.login();
    //   return false;
    // }
    // if (this.auth.tokenValid && this.auth.loggedIn) {
    //   return true;
    // }


    if (this.auth.tokenNotExpired()) {
        this.data.loggedIn(true);
        return true;
    }

    this.router.navigate(['/admin/login']);
    return false;
    
  }

}