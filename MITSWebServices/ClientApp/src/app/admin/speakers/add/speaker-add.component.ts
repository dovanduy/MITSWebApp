import { Component, OnInit, Input } from '@angular/core';

import { AllSpeakers } from '../../../graphql/generated/graphql'

@Component({
  selector: 'speaker-add',
  templateUrl: './speaker-add.component.html',
  styleUrls: ['./speaker-add.component.scss']
})
export class SpeakerAddComponent implements OnInit {

  constructor() { }

  @Input() speakers: AllSpeakers.Speakers[];
  addingSpeaker: boolean = false;

  ngOnInit() {

  }

  close(): void {
    this.addingSpeaker = false;
  }

  addSpeaker(): void {
    this.addingSpeaker = true;
  }

}
