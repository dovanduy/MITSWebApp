import { Component, OnInit } from "@angular/core";

import { ProviderService } from '../../provider/provider.service';
import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';


@Component({
  selector: "app-sponsors",
  templateUrl: "./sponsors.component.html",
  styleUrls: ["./sponsors.component.scss"]
})
export class SponsorsComponent implements OnInit {
  
  events: AllEvents.Events[];
  sponsorEvents: AllEvents.Events[];

  constructor(private allEventsGQL: AllEventsGQL, private provider: ProviderService ) {}

  ngOnInit() {
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      this.activate();

    });
  }

  activate() {
    this.sponsorEvents = this.events.filter(event => event.eventRegistrationType == "sponsor");
  }

  register(type: AllEvents.Types, eventId: number) {
   this.provider.openRegisterDialog(type, eventId);
  }
}
