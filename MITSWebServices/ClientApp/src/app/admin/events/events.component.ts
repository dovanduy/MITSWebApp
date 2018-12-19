import { Component, OnInit } from '@angular/core';
import { AdminDataService } from '../services/admin-data.service';
import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {

  constructor(private adminData: AdminDataService, private allEventsGQL: AllEventsGQL) { }

  events: AllEvents.Events[];
  showAddForm: boolean = false;
  showEditForm: boolean = false;
  activeEvent: AllEvents.Events;

  ngOnInit() {
    this.adminData.pageTitle("Events");
    this.allEventsGQL.watch().valueChanges.subscribe(results => {
      this.events = results.data.events;
      console.log(results.data.events);
    });
  }

  showEditEventForm(event: AllEvents.Events) {
    this.showEditForm = true;
    this.activeEvent = event;
  }

  addEvent(){
    this.showAddForm = true;
  }

  closeAddEventForm() {
    this.showAddForm = false;
  }

}
