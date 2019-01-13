import { Component, OnInit } from '@angular/core';

import { ProviderService } from '../../provider/provider.service';
import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private allEventsGQL: AllEventsGQL, private provider: ProviderService ) { }

  events: AllEvents.Events[];
  mainEvent: AllEvents.Events;
  golfEvent: AllEvents.Events;
  //mainEvent: AllEvents.Events;

  ngOnInit() {
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      this.activate();

    });
  }

  activate() {
    this.mainEvent = this.events.filter(event => event.eventRegistrationType == "main")[0];
    this.golfEvent = this.events.filter(event => event.eventRegistrationType == "golf")[0]
  }

  register(type: AllEvents.Types, eventId: number) {
   this.provider.openRegisterDialog(type, eventId);
  }


}
