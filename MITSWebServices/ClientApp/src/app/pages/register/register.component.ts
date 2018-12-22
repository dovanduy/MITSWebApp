import { Component, OnInit } from '@angular/core';

import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private allEventsGQL: AllEventsGQL) { }

  events: AllEvents.Events[];
  mainEvent: AllEvents.Events;

  ngOnInit() {
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      console.log(this.events);
      this.activate();

    });


  }

  activate() {
    this.events = this.events.filter(event => event.isSponsor != true);
    console.log(this.events);
  }


}
