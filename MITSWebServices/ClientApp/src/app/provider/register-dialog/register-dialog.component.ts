import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { TdDialogService } from '@covalent/core';

import { CreditCardValidator, CreditCardFormatDirective, CreditCard } from "angular-cc-library";

import {
  ProcessRegistrationGQL,
  RegistrationInput
} from "../../graphql/generated/graphql";

declare var Accept: any;
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
    private processRegistrationGQL: ProcessRegistrationGQL
  ) {
    dialogRef.disableClose = true;

  }

  isLinear = false;
  userDetailsForm: FormGroup;
  afceaDetailsForm: FormGroup;
  paymentDetailsForm: FormGroup;
  eventRegistrationType: AllEvents.Types;
  mainEventId: number;
  isFree: boolean;
  needsCode: boolean;
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

    if (input.classList.contains('identified')) {
      for (const c of cards) {
        if (input.classList.contains(c.type)) {
          return c.type;
        }
      }
    }

  }

  ngOnInit() {

    this.dialogRef.backdropClick().subscribe(data => {
      this.tdDialog.openConfirm({
        message: 'Are you sure you want to cancel this order?',
        disableClose: true,
        cancelButton: "No",
        acceptButton: "Yes, Cancel this order"        
      }).afterClosed().subscribe((accept: boolean ) => {
        if (accept) {
          this.dialogRef.close();
        }
      });
    });
    console.log(this.data);

    this.isFree = this.data.eventType.basePrice > 0 ? false : true;

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

  processRegistration(): void {
    let authData: AuthData = {
      clientKey:
        "5bmqpBJmhet82bWEg93jhwj4P3N3gYL4hy5YT7T7ZE7f5KydsZJNXTWNY3b5nXCp",
      apiLoginID: "84EchY4vL"
    };

    let cardData: CardData = {
      cardNumber: "5424000000000015",
      month: "01",
      year: "19",
      cardCode: "454"
    };

    let secureData: SecureData = {
      authData: authData,
      cardData: cardData
    };

    //.bind(this) allows the use of this in the response handler function.
    Accept.dispatchData(secureData, this.responseHandler.bind(this));
  }

  responseHandler(response: AuthorizeResponse) {
    var newRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: "Bob",
      lastName: "Anderson",
      title: "CEO, weoifjwefoji",
      email: "bob.anderson.4234@gmail.com",
      memberId: "324232423",
      memberExpirationDate: "05546",
      isLifeMember: false,
      isLocal: true,
      registrationTypeId: this.eventRegistrationType.registrationTypeId,
      eventId: this.mainEventId
    };

    console.log(newRegistration);

    this.processRegistrationGQL
      .mutate({
        registration: newRegistration
      })
      .subscribe(result => {
        this.registrationComplete = true;
        console.log(result);
        this.qrCode = result.data.processRegistration.qrCode;
        console.log(this.qrCode);
      });
  }

  print() {
    this.isFree = !this.isFree;
  }
}

interface AuthorizeResponse {
  message: AuthorizeMessage;
  opaqueData: AuthorizeOpaqueData;
}

interface AuthorizeMessage {
  message: string[];
  resultCode: string;
}

interface AuthorizeOpaqueData {
  dataDescriptor: string;
  dataValue: string;
}

interface SecureData {
  cardData: CardData;
  authData: AuthData;
}

interface CardData {
  cardNumber: string;
  month: string;
  year: string;
  cardCode: string;
}

interface AuthData {
  clientKey: string;
  apiLoginID: string;
}
