import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AllSpeakers, AllSpeakersGQL, AllSections, AllSectionsGQL } from '../../graphql/generated/graphql'
import { AdminDataService } from '../services/admin-data.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  
  sections: Observable<AllSections.Sections[]>



  constructor(private allSpeakersGQL: AllSpeakersGQL, private allSectionsGQL: AllSectionsGQL, private adminData: AdminDataService) { }

  ngOnInit() {

    this.adminData.pageTitle('Dashboard');

    console.log('It is in the detials on init');
    

    this.sections = this.allSectionsGQL
    .watch()
    .valueChanges.pipe(map(result => result.data.sections));
  }

}
