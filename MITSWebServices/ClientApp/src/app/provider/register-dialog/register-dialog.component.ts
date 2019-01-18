import { Component, OnInit, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { MatStepper } from "@angular/material/stepper";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TdDialogService, TdLoadingService } from "@covalent/core";


import {
  CreditCardValidator,
  CreditCardFormatDirective,
  CreditCard
} from "angular-cc-library";

import {
  ProcessRegistrationGQL,
  RegistrationInput
} from "../../graphql/generated/graphql";

declare var Accept: any;
import { environment } from "../../../environments/environment";
import {
  AuthData,
  CardData,
  SecureData,
  AuthorizeResponse
} from "../../core/models";
import { AllEvents } from "src/app/graphql/generated/graphql";
import { Observable, Observer, of, zip, Subject } from "rxjs";
import { takeUntil } from "rxjs/Operators";

@Component({
  selector: "register-dialog",
  templateUrl: "./register-dialog.component.html",
  styleUrls: ["./register-dialog.component.scss"]
})
export class RegisterDialogComponent implements OnInit, OnDestroy {
  constructor(
    private dialogRef: MatDialogRef<RegisterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private _formBuilder: FormBuilder,
    private tdDialog: TdDialogService,
    private tdLoading: TdLoadingService,
    private processRegistrationGQL: ProcessRegistrationGQL
  ) {
    dialogRef.disableClose = true;
  }

  isProcessingRegistration: boolean = false;
  isLinear: boolean = false;
  userDetailsForm: FormGroup;
  afceaDetailsForm: FormGroup;
  paymentDetailsForm: FormGroup;
  registrationCodeForm: FormGroup;
  eventRegistrationType: AllEvents.Types;
  isAddingTuesdayLuncheon: boolean = false;
  isCommitteeRegistration: boolean;
  mainEventId: number;
  isFree: boolean;
  codeRequired: boolean;
  qrCode: string;
  registrationComplete: boolean = false;
  tuesdayRegistration$: Observable<any>;
  mainRegistration$: Observable<any>;
  ngUnsubscribe: Subject<any> = new Subject<any>();

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

  ngOnDestroy(){
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    console.log(this.data);

    // zip(this.mainRegistration$, this.tuesdayRegistration$)
    // .pipe(takeUntil(this.ngUnsubscribe))
    // .subscribe(result => {
    //   this.tdLoading.resolve('overLayForm');
    //   console.log(result);
    // });
  

    this.dialogRef.backdropClick().subscribe(data => {
      if (!this.isProcessingRegistration) {
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
    });

    this.isFree = this.data.eventType.basePrice > 0 ? false : true;
    this.codeRequired = this.data.eventType.codeRequired;
    this.isCommitteeRegistration =
      this.data.eventType.basePrice == 0 && this.data.eventType.codeRequired;

    this.eventRegistrationType = this.data.eventType;
    this.mainEventId = this.data.mainEventId;

    this.userDetailsForm = this._formBuilder.group({
      firstName: ["", Validators.required],
      lastName: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      organization: ["", Validators.required]
    });
    this.afceaDetailsForm = this._formBuilder.group({
      memberId: [""],
      memberExpireDate: [""],
      isLifeTimeMember: [""],
      isLocal: [""]
    });
    this.registrationCodeForm = this._formBuilder.group({
      registrationCode: ["", Validators.required]
    });
    this.paymentDetailsForm = this._formBuilder.group({
      cardNumber: ["", CreditCardValidator.validateCCNumber],
      expirationDate: ["", CreditCardValidator.validateExpDate],
      cardCode: [
        "",
        [
          Validators.required,
          <any>Validators.minLength(3),
          <any>Validators.maxLength(4)
        ]
      ]
    });
  }

  changeTuesdayLuncheonOption(stepper: MatStepper) {
    this.isAddingTuesdayLuncheon = !this.isAddingTuesdayLuncheon;
  }

  

  processRegistration(): void {
    //transfer this to an environment variable
    this.tdLoading.register("overLayForm");
    this.isProcessingRegistration = true;

    
    

    //If this is a Government Registration don't process a credit card
    if (this.isFree) {

      of(this.mainRegistration$);

      var newFreeRegistration = this.createNewFreeRegistration()
      console.log(newFreeRegistration);
      //this.mainRegistration$ = this.sendRegistrationToServer(newFreeRegistration);
    } 
    
    else {
      //var secureData = this.createSecureData();
      if (this.isAddingTuesdayLuncheon) {
        //Accept.dispatchData(secureData, this.responseTuesdayHandler.bind(this));
      }
      //.bind(this) allows the use of this in the response handler function.
      //Accept.dispatchData(secureData, this.responseMainHandler.bind(this));
    }

    
  }

  createNewTuesdayLuncheonRegistration(
    response: AuthorizeResponse
  ): RegistrationInput {
    var newLuncheonRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: this.userDetailsForm.controls.firstName.value,
      lastName: this.userDetailsForm.controls.lastName.value,
      organization: this.userDetailsForm.controls.organization.value,
      email: this.userDetailsForm.controls.email.value,
      memberId: this.afceaDetailsForm.controls.memberId.value,
      memberExpirationDate: this.afceaDetailsForm.controls.memberExpireDate
        .value,
      isLifeMember: this.afceaDetailsForm.controls.isLifeTimeMember.value,
      isLocal: this.afceaDetailsForm.controls.isLocal.value,
      registrationTypeId: this.eventRegistrationType.registrationTypeId,
      eventId: this.mainEventId
    };

    return newLuncheonRegistration;
  }

  createNewFreeRegistration(): RegistrationInput {
    var newFreeRegistration: RegistrationInput = {
      dataDescriptor: "",
      dataValue: "",
      registrationCode: this.registrationCodeForm.controls.registrationCode.value,
      firstName: this.userDetailsForm.controls.firstName.value,
      lastName: this.userDetailsForm.controls.lastName.value,
      organization: this.userDetailsForm.controls.organization.value,
      email: this.userDetailsForm.controls.email.value,
      memberId: this.afceaDetailsForm.controls.memberId.value,
      memberExpirationDate: this.afceaDetailsForm.controls.memberExpireDate
        .value,
      isLifeMember: this.afceaDetailsForm.controls.isLifeTimeMember.value || false,
      isLocal: this.afceaDetailsForm.controls.isLocal.value || false,
      registrationTypeId: this.eventRegistrationType.registrationTypeId,
      eventId: this.mainEventId
    };

    return newFreeRegistration;
  }

  createNewMainRegistration(response: AuthorizeResponse): RegistrationInput {
    var newMainRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: this.userDetailsForm.controls.firstName.value,
      lastName: this.userDetailsForm.controls.lastName.value,
      organization: this.userDetailsForm.controls.organization.value,
      email: this.userDetailsForm.controls.email.value,
      memberId: this.afceaDetailsForm.controls.memberId.value,
      memberExpirationDate: this.afceaDetailsForm.controls.memberExpireDate
        .value,
      isLifeMember: this.afceaDetailsForm.controls.isLifeTimeMember.value,
      isLocal: this.afceaDetailsForm.controls.isLocal.value,
      registrationTypeId: this.eventRegistrationType.registrationTypeId,
      eventId: this.mainEventId
    };

    return newMainRegistration;
  }

 

  responseTuesdayHandler(response: AuthorizeResponse) {
    console.log('Tuesday Authorize Response');
    console.log(response);

    if (response.messages.resultCode === "Error") {
      var errorMessage = response.messages.message[0].text;
      this.tdDialog.openAlert({
        message: "Error processing Credit Card, please try again",
        title: "Error"
      });
    } else {
      
      var newTuesdayRegistration = this.createNewTuesdayLuncheonRegistration(response);
      //this.tuesdayRegistration$ = this.sendRegistrationToServer(newTuesdayRegistration);
      //var secureData = this.createSecureData();
      //Accept.dispatchData(secureData, this.responseMainHandler.bind(this));
      
    }
  }

  responseMainHandler(response: AuthorizeResponse) {
    console.log('Main authorize response');
    console.log(response);

    if (response.messages.resultCode === "Error") {
      var errorMessage = response.messages.message[0].text;
      this.tdDialog.openAlert({
        message: "Error processing Credit Card, please try again",
        title: "Error"
      });
    }
    if (!this.isAddingTuesdayLuncheon) {
      of(this.tuesdayRegistration$);
    }

    var newMainRegistration = this.createNewMainRegistration(response);
    //this.mainRegistration$ = this.sendRegistrationToServer(newMainRegistration);
    
  }
}
