import { Component, OnInit } from '@angular/core';
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'app-sections',
  templateUrl: './sections.component.html',
  styleUrls: ['./sections.component.scss']
})
export class SectionsComponent implements OnInit {

  constructor(private adminData: AdminDataService) { }

  ngOnInit() {
    this.adminData.pageTitle('Sections');
  }

}
