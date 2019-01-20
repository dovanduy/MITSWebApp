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
import { GraphQLProcessRegistrationResponse, AuthorizeResponse } from "../../core/models";

@Component({
  selector: 'app-industry-register-dialog',
  templateUrl: './industry-register-dialog.component.html',
  styleUrls: ['./industry-register-dialog.component.scss']
})
export class IndustryRegisterDialogComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<IndustryRegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private tdDialog: TdDialogService,
    private tdLoading: TdLoadingService,
    private registerService: RegisterService) {
    dialogRef.disableClose = true;
  }

  userDetailsForm: FormGroup;
  paymentDetailsForm: FormGroup;
  afceaDetailsForm: FormGroup;
  registrationComplete: boolean = false;
  isProcessingRegistration: boolean = false;
  qrCode: string;
  eventRegistrationId: number;

  get udForm() {
    return this.userDetailsForm.controls;
  }

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
    console.log(this.userDetailsForm);

    this.paymentDetailsForm = this.registerService.createPaymentDetailsFormGroup();
    console.log(this.paymentDetailsForm);

    if (this.data.isAfcean) {
      this.afceaDetailsForm = this.registerService.createAfceaDetailsFormGroup();
    }

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

    var secureData = this.registerService.createSecureData(this.paymentDetailsForm);

    this.dispatchCCData(secureData);


  }

  finishRegistration(ccAuthData: AuthorizeResponse): void {

    var newReg = {};



    if (this.data.isAfcean) {
      newReg = this.registerService.createNewPaymentAfceanRegistration(
        ccAuthData,
        this.userDetailsForm,
        this.data.eventType,
        this.data.mainEventId,
        this.afceaDetailsForm
      )

    } else {
      newReg = this.registerService.createNewPaymentRegistration(
        ccAuthData,
        this.userDetailsForm,
        this.data.eventType,
        this.data.mainEventId
      );
    }

    console.log(newReg);
    this.tdLoading.resolve('overLayForm');

    //   this.registerService
    //     .sendRegistrationToServer(newReg)
    //     .subscribe((result: GraphQLProcessRegistrationResponse) => {
    //       console.log(result);
    //       this.tdLoading.resolve("overLayForm");
    //       this.isProcessingRegistration = false;

    //       if (result.errors === undefined) {
    //         this.qrCode = result.data.processRegistration.qrCode;
    //         this.eventRegistrationId =
    //           result.data.processRegistration.eventRegistrationId;

    //         this.registrationComplete = true;
    //       } else {
    //         console.log(result.errors[0].message);
    //         var message = result.errors[0].message;
    //         this.tdDialog.openAlert({
    //           title: "Sorry, there was a problem processing your registration.",
    //           message: message
    //         });
    //       }
    //     });
  }

}
