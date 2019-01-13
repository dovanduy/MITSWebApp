import { Component, OnInit, Inject, ViewChild } from "@angular/core";
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

@Component({
  selector: "register-dialog",
  templateUrl: "./register-dialog.component.html",
  styleUrls: ["./register-dialog.component.scss"]
})
export class RegisterDialogComponent implements OnInit {
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
    console.log(this.data);

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
      title: ["", Validators.required]
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
    
    let authData: AuthData = {
      clientKey: environment.clientKey,
      apiLoginID: environment.apiLoginID
    };

    var ccNumber = this.paymentDetailsForm.controls.cardNumber.value;
    var ccCcv = this.paymentDetailsForm.controls.cardCode.value;
    var ccDetails = this.paymentDetailsForm.controls.expirationDate.value.split(
      "/"
    );
    var ccMonth = ccDetails[0].trim();
    var ccYear = ccDetails[1].trim();
    if (ccYear.length > 2) {
      ccYear = ccYear.substring(2);
    }

    // let cardData: CardData = {
    //   cardNumber: "5424000000000015",
    //   month: "01",
    //   year: "19",
    //   cardCode: "454"
    // };

    let cardData: CardData = {
      cardNumber: ccNumber,
      month: ccMonth,
      year: ccYear,
      cardCode: ccCcv
    };

    let secureData: SecureData = {
      authData: authData,
      cardData: cardData
    };

    //.bind(this) allows the use of this in the response handler function.
    Accept.dispatchData(secureData, this.responseHandler.bind(this));
  }

  responseHandler(response: AuthorizeResponse) {
    console.log(response);

    if (response.messages.resultCode === "Error") {
      var errorMessage = response.messages.message[0].text;
      this.tdDialog.openAlert({
        message: "Error processing Credit Card, please try again",
        title: "Error"
      });
    }

    var newRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: this.userDetailsForm.controls.firstName.value,
      lastName: this.userDetailsForm.controls.lastName.value,
      title: this.userDetailsForm.controls.title.value,
      email: this.userDetailsForm.controls.email.value,
      memberId: this.afceaDetailsForm.controls.memberId.value,
      memberExpirationDate: this.afceaDetailsForm.controls.memberExpireDate
        .value,
      isLifeMember: this.afceaDetailsForm.controls.isLifeTimeMember.value,
      isLocal: this.afceaDetailsForm.controls.isLocal.value,
      registrationTypeId: this.eventRegistrationType.registrationTypeId,
      eventId: this.mainEventId
    };

    console.log(newRegistration);

    // this.processRegistrationGQL
    //   .mutate({
    //     registration: newRegistration
    //   })
    //   .subscribe(result => {
    //     this.registrationComplete = true;
    //     console.log(result);
    //     this.qrCode = result.data.processRegistration.qrCode;
    //     console.log(this.qrCode);
    //   });
  }
}
