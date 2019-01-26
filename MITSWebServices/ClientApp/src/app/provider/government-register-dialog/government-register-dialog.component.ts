import { Component, OnInit, Inject, ViewChild, OnDestroy } from "@angular/core";
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
  AuthorizeResponse,
  Ticket
} from "../../core/models";
import { RegistrationInput } from "src/app/graphql/generated/graphql";
import { Observable, Subject, zip } from "rxjs";
import { takeUntil } from "rxjs/Operators";

@Component({
  selector: "app-government-register-dialog",
  templateUrl: "./government-register-dialog.component.html",
  styleUrls: ["./government-register-dialog.component.scss"]
})
export class GovernmentRegisterDialogComponent implements OnInit, OnDestroy {
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
  afceaDetailsValid: boolean = true;
  isAddingLuncheon: boolean = false;
  qrCode: string;
  eventRegistrationId: number;
  luncheonRegistationId: number;
  luncheonQrCode: string;
  mainRegistration$: Observable<GraphQLProcessRegistrationResponse>;
  tuesdayRegistration$: Observable<GraphQLProcessRegistrationResponse>;
  ngUnsubscribe: Subject<any> = new Subject<any>();
  tickets: Ticket[] = [];

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

  ngOnDestroy() {
    //Clean up the drop subscription to prevent errors.
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
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
    this.afceaDetailsForm = this.registerService.createAfceaDetailsFormGroup(
      true
    );
    console.log(this.afceaDetailsForm);
    this.paymentDetailsForm = this.registerService.createPaymentDetailsFormGroup();
    console.log(this.paymentDetailsForm);

    this.afceaDetailsForm.get("memberId").valueChanges.subscribe(data => {
      console.log(data);
      if (this.afceaDetailsForm.get("memberId").value == "") {
        this.afceaDetailsForm.get("memberExpireDate").setValidators([]);
        this.afceaDetailsForm.get("memberExpireDate").updateValueAndValidity();
      } else {
        this.afceaDetailsForm
          .get("memberExpireDate")
          .setValidators([Validators.required]);
        this.afceaDetailsForm.get("memberExpireDate").updateValueAndValidity();
      }
    });
  }

  changeLuncheonOption(stepper: MatStepper) {
    this.isAddingLuncheon = !this.isAddingLuncheon;
  }

  startRegistration() {
    console.log("Registration witout tuesday luncheon");
    this.tdLoading.register("overLayForm");
    this.isProcessingRegistration = true;
    var newReg = this.registerService.createNewGovernmentRegistration(
      this.userDetailsForm,
      this.data.eventType,
      this.data.mainEventId,
      this.afceaDetailsForm
    );

    console.log(newReg);

    this.registerService
      .sendRegistrationToServer(newReg)
      .subscribe((result: GraphQLProcessRegistrationResponse) => {
        console.log(result);
        this.tdLoading.resolve("overLayForm");
        this.isProcessingRegistration = false;

        if (result.errors === undefined) {
          this.registrationComplete = true;
          // this.qrCode = result.data.processRegistration.qrCode;
          // this.eventRegistrationId =
          //   result.data.processRegistration.eventRegistrationId;
          this.tickets.push( {
            eventRegistrationId: result.data.processRegistration.eventRegistrationId,
            qrCode: result.data.processRegistration.qrCode,
            event: this.data.eventType

          } as Ticket);
          
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

  startTuesdayRegistration() {
    console.log("Registration with tuesdaey luncheon");
    this.tdLoading.register("overLayForm");
    this.isProcessingRegistration = true;
    var newMainReg = this.registerService.createNewGovernmentRegistration(
      this.userDetailsForm,
      this.data.eventType,
      this.data.mainEventId,
      this.afceaDetailsForm
    );

    this.mainRegistration$ = this.registerService.sendRegistrationToServer(
      newMainReg
    );

    var secureData = this.registerService.createSecureData(
      this.paymentDetailsForm
    );

    this.dispatchCCData(secureData);
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

    this.finishTuesdayRegistration(response);
  }

  finishTuesdayRegistration(ccAuthData: AuthorizeResponse) {
    var newTuesdayRegistration = this.registerService.createNewTuesdayLuncheonRegistration(
      ccAuthData,
      this.userDetailsForm,
      this.data.luncheonEvent.waEvent[0].types[0],
      this.luncheonEventId
    );

    this.tuesdayRegistration$ = this.registerService.sendRegistrationToServer(
      newTuesdayRegistration
    );

    zip(this.mainRegistration$, this.tuesdayRegistration$)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((results: GraphQLProcessRegistrationResponse[]) => {
        console.log(results);
        this.tdLoading.resolve("overLayForm");
        this.isProcessingRegistration = false;
        this.registrationComplete = true;
        // this.eventRegistrationId =
        //     result[0].data.processRegistration.eventRegistrationId;
        // this.qrCode = result[0].data.processRegistration.qrCode;

        // this.luncheonEventId =
        //     result[1].data.processRegistration.eventRegistrationId;
        // this.luncheonQrCode = result[1].data.processRegistration.qrCode;

        this.tickets.push( {
          eventRegistrationId: results[0].data.processRegistration.eventRegistrationId,
          qrCode: results[0].data.processRegistration.qrCode,
          event: this.data.eventType

        } as Ticket);

        this.tickets.push( {
          eventRegistrationId: results[1].data.processRegistration.eventRegistrationId,
          qrCode: results[1].data.processRegistration.qrCode,
          event: this.data.luncheonEvent.waEvent[0].types[0]

        } as Ticket);

        

      });
  }
}
