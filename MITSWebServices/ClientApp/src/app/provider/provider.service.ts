import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject } from "rxjs";

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import { RegisterDialogComponent } from "./register-dialog/register-dialog.component";
import { AllEvents } from 'src/app/graphql/generated/graphql';

@Injectable({
  providedIn: "root"
})
export class ProviderService {
  dialogRef: MatDialogRef<RegisterDialogComponent>;
  registerDialogClosed$: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(private dialog: MatDialog) {}


  openRegisterDialog(eventType: AllEvents.Types, mainEventId: number ): Observable<MatDialogRef<RegisterDialogComponent>> {
    return this.dialog.open(RegisterDialogComponent, {
      height: 'auto',
      width: '500px',
      data: {
        eventType: eventType,
        mainEventId: mainEventId
      }
    }).afterClosed();
  }

  //closeRegisterDialog()
}
