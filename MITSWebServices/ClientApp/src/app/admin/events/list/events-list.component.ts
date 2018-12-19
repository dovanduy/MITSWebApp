import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AllEvents, AllEventsGQL } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'events-list',
  templateUrl: './events-list.component.html',
  styleUrls: ['./events-list.component.scss']
})
export class EventsListComponent implements OnInit {

  constructor() { }

  @Input() events: AllEvents.Events[]
  @Output() onEdit = new EventEmitter<AllEvents.Events>()
  activeEvent: AllEvents.Events;

  ngOnInit() {
    console.log(this.events);
  }

  editing(event: AllEvents.Events) {
    this.activeEvent = event;
    this.onEdit.emit(event);
  }

}
