import { Component, OnInit } from "@angular/core";

import { RegisterDialogService } from "../../provider/services/register-dialog.service";
import { AllEvents, AllEventsGQL } from "src/app/graphql/generated/graphql";
import { Observable } from "rxjs";
import { GlobalService } from "src/app/core/services/global.service";
import { TdLoadingService } from "@covalent/core";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"]
})
export class RegisterComponent implements OnInit {
  constructor(
    private registerDialogService: RegisterDialogService,
    private globalService: GlobalService,
    private allEventsGQL: AllEventsGQL,
    private tdLoading: TdLoadingService
  ) {}

  events$: Observable<any[]>;
  events: AllEvents.Events[];
  mainEvent: AllEvents.Events;
  golfEvent: AllEvents.Events;
  luncheonEvent: AllEvents.Events;

  ngOnInit() {
    this.tdLoading.register();
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      this.activate();
    });
  }

  activate() {
    this.mainEvent = this.events.filter(
      event => event.eventRegistrationType === "main"
    )[0];
    this.golfEvent = this.events.filter(
      event => event.eventRegistrationType === "golf"
    )[0];
    this.luncheonEvent = this.events.filter(
      event => event.eventRegistrationType === "luncheon"
    )[0];

    this.tdLoading.resolve();
  }

  registerMain(type: AllEvents.Types, eventId: number) {
    //Registration for Commitee Member
    if (type.basePrice === 0 && type.codeRequired) {
      console.log("This is registration for commitee member");
      this.registerDialogService.openCommitteeRegistrationDialog(type, eventId);
    }

    //Registration for Government Attendee
    if (type.basePrice === 0 && !type.codeRequired) {
      console.log("This is registration for Government member");
      this.registerDialogService.openGovernmentRegistrationDialog(
        type,
        eventId
      );
    }

    //Registration for Industry Attendee
    if (type.basePrice > 0) {
      console.log("This is registration for industry member");

      //Registration for AFCEAN Member Industry Attendee
      if (type.name.match(/Non/g) == null) {
        console.log("This is an afceaon Registration");
        this.registerDialogService.openIndustryRegistrationDialog(
          type,
          eventId,
          true
        );
        //Registration for Non-AFCEAN Member Industry Attendee
      } else {
        console.log("This is a non-afcean registration");
        this.registerDialogService.openIndustryRegistrationDialog(
          type,
          eventId,
          false
        );
      }
    }
  }

  registerGolf(type: AllEvents.Types, eventId: number) {
    console.log("This is a golf registration");
    console.log(type);
    this.registerDialogService.openRegisterDialog(type, eventId, true);
  }

  registerLuncheon(type: AllEvents.Types, eventId: number) {
    console.log("This is a Luncheon registration");
    console.log(type);
    this.registerDialogService.openRegisterDialog(type, eventId, false);
  }
}
