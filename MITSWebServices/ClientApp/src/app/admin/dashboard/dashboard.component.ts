import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AllSpeakers, AllSpeakersGQL, AllSections, AllSectionsGQL } from '../../graphql/generated/graphql'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  speakers: Observable<AllSpeakers.Speakers[]>
  sections: Observable<AllSections.Sections[]>

  constructor(private allSpeakersGQL: AllSpeakersGQL, private allSectionsGQL: AllSectionsGQL) { }

  ngOnInit() {
    console.log('It is in the detials on init');
    this.speakers = this.allSpeakersGQL
    .watch()
    .valueChanges.pipe(map(result => {
      console.log(result.data.speakers)
      return result.data.speakers
    }));

    this.sections = this.allSectionsGQL
    .watch()
    .valueChanges.pipe(map(result => result.data.sections));
  }

}
