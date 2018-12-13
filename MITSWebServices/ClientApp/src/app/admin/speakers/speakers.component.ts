import { Component, OnInit } from '@angular/core';
import { AdminDataService } from '../services/admin-data.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { AllSpeakers, AllSpeakersGQL } from '../../graphql/generated/graphql'

@Component({
  selector: 'app-speakers',
  templateUrl: './speakers.component.html',
  styleUrls: ['./speakers.component.scss']
})
export class SpeakersComponent implements OnInit {

  constructor(private adminData: AdminDataService, private allSpeakersGQL: AllSpeakersGQL) { }
  
  speakers: Observable<AllSpeakers.Speakers[]>

  ngOnInit() {
    this.adminData.pageTitle('Speakers');

    this.speakers = this.allSpeakersGQL
    .watch()
    .valueChanges.pipe(map(result => {
      console.log(result.data.speakers)
      return result.data.speakers
    }));
  }

}
