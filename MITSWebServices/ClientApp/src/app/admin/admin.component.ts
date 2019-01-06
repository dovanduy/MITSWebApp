import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AdminDataService } from './services/admin-data.service';
import { map, share } from 'rxjs/operators';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  
  pageTitle$: Observable<String>;
  loggedIn$: Observable<boolean>;

  constructor(private adminData: AdminDataService) { }

  ngOnInit() {
    this.pageTitle$ = this.adminData.pageTitle$.pipe(share());
    this.loggedIn$ = this.adminData.loggedIn$.pipe(share());
  }

}
