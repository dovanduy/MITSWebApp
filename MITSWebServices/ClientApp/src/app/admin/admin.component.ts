import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AdminDataService } from './services/admin-data.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  
  pageTitle: String;
  pageTitle$: Observable<String>

  constructor(private adminData: AdminDataService) { }

  ngOnInit() {
    this.pageTitle$ = this.adminData.pageTitle$.pipe(map((data) => this.pageTitle = data ));
  }

}
