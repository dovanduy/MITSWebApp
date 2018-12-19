import { Component, OnInit, Input } from '@angular/core';

import { AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.scss']
})
export class EventEditComponent implements OnInit {

  constructor() { }

  @Input() event: AllEvents.Events

  ngOnInit() {
  }

}
