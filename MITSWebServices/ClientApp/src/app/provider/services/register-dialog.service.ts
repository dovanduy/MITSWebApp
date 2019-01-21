import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject } from "rxjs";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import { RegisterDialogComponent } from "../register-dialog/register-dialog.component";
import { CommitteeRegisterDialogComponent } from "../committee-register-dialog/committee-register-dialog.component";
import { GovernmentRegisterDialogComponent } from "../government-register-dialog/government-register-dialog.component";
import { IndustryRegisterDialogComponent } from "../industry-register-dialog/industry-register-dialog.component";
import { SponsorRegisterDialogComponent } from '../sponsor-register-dialog/sponsor-register-dialog.component';

import {
  AllEvents,
  ProcessRegistrationGQL,
  RegistrationInput
} from "src/app/graphql/generated/graphql";
import { TdDialogService } from "@covalent/core";

@Injectable({
  providedIn: "root"
})
export class RegisterDialogService {
  // dialogRef: MatDialogRef<RegisterDialogComponent>;
  // registerDialogClosed$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(
    private dialog: MatDialog,
    
  ) {}

  //Used for Golf Registration and Tuesday Luncheon
  openRegisterDialog(
    eventType: AllEvents.Types,
    mainEventId: number,
    isGolf: boolean
  ): Observable<MatDialogRef<RegisterDialogComponent>> {
    return this.dialog
      .open(RegisterDialogComponent, {
        height: "auto",
        width: "500px",
        data: {
          eventType: eventType,
          mainEventId: mainEventId,
          isGolf: isGolf
        }
      })
      .afterClosed();
  }

  openCommitteeRegistrationDialog(
    eventType: AllEvents.Types,
    mainEventId: number
  ): Observable<MatDialogRef<CommitteeRegisterDialogComponent>> {
    return this.dialog
      .open(CommitteeRegisterDialogComponent, {
        height: "auto",
        width: "500px",
        data: {
          eventType: eventType,
          mainEventId: mainEventId
        }
      })
      .afterClosed();
  }

  openGovernmentRegistrationDialog(
    eventType: AllEvents.Types,
    mainEventId: number,
    luncheonEvent: AllEvents.Events
  ): Observable<MatDialogRef<GovernmentRegisterDialogComponent>> {
    return this.dialog
      .open(GovernmentRegisterDialogComponent, {
        height: "auto",
        width: "500px",
        data: {
          eventType: eventType,
          mainEventId: mainEventId,
          luncheonEvent: luncheonEvent
        }
      })
      .afterClosed();
  }

  openIndustryRegistrationDialog(
    eventType: AllEvents.Types,
    mainEventId: number,
    isAfcean: boolean
  ): Observable<MatDialogRef<IndustryRegisterDialogComponent>> {
    return this.dialog
      .open(IndustryRegisterDialogComponent, {
        height: "auto",
        width: "500px",
        data: {
          eventType: eventType,
          mainEventId: mainEventId,
          isAfcean: isAfcean
        }
      })
      .afterClosed();
  }

  openSponsorRegistrationDialog(
    eventType: AllEvents.Types,
    sponsorEventId: number
  ): Observable<MatDialogRef<SponsorRegisterDialogComponent>> {
    return this.dialog
      .open(SponsorRegisterDialogComponent, {
        height: "auto",
        width: "500px",
        data: {
          eventType: eventType,
          sponsorEventId: sponsorEventId,
        }
      })
      .afterClosed();
  }


  
  
}
