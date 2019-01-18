import { Component, OnInit } from "@angular/core";

import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';
import { RegisterDialogService } from "src/app/provider/services/register-dialog.service";


@Component({
  selector: "app-sponsors",
  templateUrl: "./sponsors.component.html",
  styleUrls: ["./sponsors.component.scss"]
})
export class SponsorsComponent implements OnInit {
  
  events: AllEvents.Events[];
  sponsorEvents: AllEvents.Events[];

  constructor(private allEventsGQL: AllEventsGQL, private registerDialogService: RegisterDialogService ) {}

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
   //this.provider.openRegisterDialog(type, eventId);
  }
}
