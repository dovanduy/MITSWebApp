import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

import {
  CreditCardValidator,
  CreditCardFormatDirective,
  CreditCard
} from "angular-cc-library";

declare var Accept: any;

import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TdDialogService, TdLoadingService } from "@covalent/core";
import { RegisterService } from "../services/register.service";
import {
  GraphQLProcessRegistrationResponse,
  AuthorizeResponse,
  GraphQLProcessSponsorResponse
} from "../../core/models";
import { RegistrationInput } from "src/app/graphql/generated/graphql";

@Component({
  selector: "app-sponsor-register-dialog",
  templateUrl: "./sponsor-register-dialog.component.html",
  styleUrls: ["./sponsor-register-dialog.component.scss"]
})
export class SponsorRegisterDialogComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<SponsorRegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private tdDialog: TdDialogService,
    private tdLoading: TdLoadingService,
    private registerService: RegisterService
  ) {
    dialogRef.disableClose = true;
  }

  isProcessingRegistration: boolean = false;
  registrationComplete: boolean = false;
  userDetailsForm: FormGroup;
  paymentDetailsForm: FormGroup;
  eventCost: number;
  eventRegistrationId: number;

  @ViewChild(CreditCardFormatDirective) ccCardInput: CreditCardFormatDirective;

  get cardBrand() {
    if (!this.ccCardInput) {
      return;
    }

    const input = <HTMLInputElement>this.ccCardInput.target;
    const cards = CreditCard.cards();

    if (input.classList.contains("identified")) {
      for (const c of cards) {
        if (input.classList.contains(c.type)) {
          return c.type;
        }
      }
    }
  }

  get udForm() {
    return this.userDetailsForm.controls;
  }

  ngOnInit() {
    console.log(this.data);
    this.eventCost = this.data.eventType.basePrice;

    this.userDetailsForm = this.registerService.createUserDetailsFormGroup();
    console.log(this.userDetailsForm);

    this.paymentDetailsForm = this.registerService.createPaymentDetailsFormGroup();
    console.log(this.paymentDetailsForm);

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
  }

  dispatchCCData(secureData): void {
    Accept.dispatchData(secureData, this.responseCCHandler.bind(this));
  }

  responseCCHandler(response: AuthorizeResponse): void {
    console.log(response);

    if (response.messages.resultCode === "Error") {
      var errorMessage = response.messages.message[0].text;
      this.tdDialog.openAlert({
        message: "Error processing Credit Card, please try again",
        title: "Error"
      });
    }

    this.finishRegistration(response);
  }

  startRegistration() {
    this.tdLoading.register("overLayForm");
    this.isProcessingRegistration = true;

    var secureData = this.registerService.createSecureData(
      this.paymentDetailsForm
    );

    this.dispatchCCData(secureData);
  }

  finishRegistration(ccAuthData: AuthorizeResponse): void {
    var newReg = this.registerService.createNewSponsorRegistration(
      ccAuthData,
      this.userDetailsForm,
      this.data.eventType,
      this.data.sponsorEventId
    );

    console.log(newReg);

    this.registerService
      .sendSponsorRegistrationToServer(newReg)
      .subscribe((result: GraphQLProcessSponsorResponse) => {
        console.log(result);
        this.tdLoading.resolve("overLayForm");
        this.isProcessingRegistration = false;

        if (result.errors === undefined) {
          this.eventRegistrationId =
            result.data.processSponsorRegistration.eventRegistrationId;
          this.registrationComplete = true;
        } else {
          console.log(result.errors[0].message);
          var message = result.errors[0].message;

          if (
            message == "The registration limit for this event has been reached."
          ) {
            this.tdDialog
              .openAlert({
                title: "Sorry, You just missed it",
                message: message
              })
              .afterClosed()
              .subscribe(data => {
                this.dialogRef.close(this.data.sponsorEventId);
              });
          } else {
            this.tdDialog.openAlert({
              title: "Sorry, there was a problem processing your registration.",
              message: message
            });
          }
        }
      });
  }
}
