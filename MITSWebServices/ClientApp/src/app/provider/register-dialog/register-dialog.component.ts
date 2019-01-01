import { Component, OnInit, Inject } from "@angular/core";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import { ProcessRegistrationGQL, RegistrationInput } from '../../graphql/generated/graphql';

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
    private processRegistrationGQL: ProcessRegistrationGQL
  ) {}

  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;
  eventRegistrationType: AllEvents.Types;
  mainEventId: number;
  qrCode: string;
  registrationComplete: boolean = false;

  ngOnInit() {

    this.eventRegistrationType = this.data.eventType;
    this.mainEventId = this.data.mainEventId;

    console.log(this.eventRegistrationType);
    console.log(this.mainEventId);

    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: ["", Validators.required]
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: ["", Validators.required]
    });
    this.thirdFormGroup = this._formBuilder.group({
      thirdCtrl: ["", Validators.required]
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
      firstName: 'Bob',
      lastName: 'Anderson',
      title: 'CEO, weoifjwefoji',
      email: 'bob.anderson@gmail.com',
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
    }).subscribe(result => {
      this.registrationComplete = true;
      console.log(result)
      this.qrCode = result.data.processRegistration.qrCode;
      console.log(this.qrCode);
    });

    

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
