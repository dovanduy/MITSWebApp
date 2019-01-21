import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA, MatStepper } from "@angular/material";

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
  AuthorizeResponse
} from "../../core/models";
import { RegistrationInput } from "src/app/graphql/generated/graphql";

@Component({
  selector: "app-government-register-dialog",
  templateUrl: "./government-register-dialog.component.html",
  styleUrls: ["./government-register-dialog.component.scss"]
})
export class GovernmentRegisterDialogComponent implements OnInit {
  constructor(
    private dialogRef: MatDialogRef<GovernmentRegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private tdDialog: TdDialogService,
    private tdLoading: TdLoadingService,
    private registerService: RegisterService
  ) {
    dialogRef.disableClose = true;
  }

  userDetailsForm: FormGroup;
  afceaDetailsForm: FormGroup;
  paymentDetailsForm: FormGroup;
  registrationComplete: boolean = false;
  isProcessingRegistration: boolean = false;
  isAddingLuncheon: boolean = false;
  qrCode: string;
  eventRegistrationId: number;


  luncheonName: string;
  luncheonEventId: number;
  luncheonCost: number;
  luncheonDate: Date;

  get udForm() {
    return this.userDetailsForm.controls;
  }

  get adForm() {
    return this.afceaDetailsForm.controls;
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

    this.luncheonName = this.data.luncheonEvent.waEvent[0].name;
    this.luncheonDate = this.data.luncheonEvent.waEvent[0].startDate;
    this.luncheonCost = this.data.luncheonEvent.waEvent[0].types[0].basePrice;
    this.luncheonEventId = this.data.luncheonEvent.mainEventId;
    
    console.log(this.luncheonName);
    console.log(this.luncheonDate);
    console.log(this.luncheonCost);
    console.log(this.luncheonEventId);


    this.userDetailsForm = this.registerService.createUserDetailsFormGroup();
    console.log(this.userDetailsForm);
    this.afceaDetailsForm = this.registerService.createAfceaDetailsFormGroup(true);
    console.log(this.afceaDetailsForm);
    this.paymentDetailsForm = this.registerService.createPaymentDetailsFormGroup();
    console.log(this.paymentDetailsForm);
    
    
  }

  changeLuncheonOption(stepper: MatStepper) {
    this.isAddingLuncheon = !this.isAddingLuncheon;
  }

  processRegistration() {}
}
