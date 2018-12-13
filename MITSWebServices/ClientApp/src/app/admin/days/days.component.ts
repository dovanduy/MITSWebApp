import { Component, OnInit } from '@angular/core';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'app-days',
  templateUrl: './days.component.html',
  styleUrls: ['./days.component.scss']
})
export class DaysComponent implements OnInit {

  constructor(private adminData: AdminDataService) { }

  ngOnInit() {
    this.adminData.pageTitle("Days");
  }

}
