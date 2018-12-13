import { Component, OnInit } from '@angular/core';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'app-sponsors',
  templateUrl: './sponsors.component.html',
  styleUrls: ['./sponsors.component.scss']
})
export class SponsorsComponent implements OnInit {

  constructor(private adminData: AdminDataService) { }

  ngOnInit() {
    this.adminData.pageTitle('Sponsors');
  }

}
