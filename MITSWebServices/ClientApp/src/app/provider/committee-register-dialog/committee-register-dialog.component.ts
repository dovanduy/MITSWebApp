import { Component, OnInit, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TdDialogService, TdLoadingService } from "@covalent/core";
import { RegisterService } from "../services/register.service";
import { GraphQLProcessRegistrationResponse } from "../../core/models";

@Component({
  selector: "app-committee-register-dialog",
  templateUrl: "./committee-register-dialog.component.html",
  styleUrls: ["./committee-register-dialog.component.scss"]
})
export class CommitteeRegisterDialogComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<CommitteeRegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private tdDialog: TdDialogService,
    private tdLoading: TdLoadingService,
    private registerService: RegisterService
  ) {
    dialogRef.disableClose = true;
  }

  userDetailsForm: FormGroup;
  registrationCodeForm: FormGroup;
  registrationComplete: boolean = false;
  isProcessingRegistration: boolean = false;
  qrCode: string;
  eventRegistrationId: number;

  get udForm() {
    return this.userDetailsForm.controls;
  }

  ngOnInit() {
    this.dialogRef.backdropClick().subscribe(data => {
      if (
        !this.isProcessingRegistration &&
        this.registrationComplete == false
      ) {
        this.tdDialog
          .openConfirm({
            message: "Are you sure you want to cancel this order?",
            disableClose: true,
            cancelButton: "No",
            acceptButton: "Yes, Cancel this order"
          })
          .afterClosed()
          .subscribe((accept: boolean) => {
            if (accept) {
              this.dialogRef.close();
            }
          });
      }

      if (this.registrationComplete) {
        this.dialogRef.close();
      }
    });

    console.log(this.data);

    this.userDetailsForm = this.registerService.createUserDetailsFormGroup();

    this.registrationCodeForm = this._formBuilder.group({
      registrationCode: ["", Validators.required]
    });

    console.log(this.userDetailsForm);
  }

  processRegistration() {
    this.tdLoading.register("overLayForm");
    this.isProcessingRegistration = true;

    var newReg = this.registerService.createNewCommitteeRegistration(
      this.userDetailsForm,
      this.registrationCodeForm,
      this.data.eventType,
      this.data.mainEventId
    );

    console.log(newReg);

    this.registerService
      .sendRegistrationToServer(newReg)
      .subscribe((result: GraphQLProcessRegistrationResponse) => {
        console.log(result);
        this.tdLoading.resolve("overLayForm");
        this.isProcessingRegistration = false;

        if (result.errors === undefined) {
          this.qrCode = result.data.processRegistration.qrCode;
          this.eventRegistrationId =
            result.data.processRegistration.eventRegistrationId;

          this.registrationComplete = true;
        } else {
          console.log(result.errors[0].message);
          var message = result.errors[0].message;
          this.tdDialog.openAlert({
            title: "Sorry, there was a problem processing your registration.",
            message: message
          });
        }
      });
  }
}
