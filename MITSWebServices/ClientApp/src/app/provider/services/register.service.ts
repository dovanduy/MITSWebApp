import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { CreditCardValidator } from "angular-cc-library";

import {
  AllEvents,
  ProcessRegistrationGQL,
  ProcessSponsorRegistrationGQL,
  RegistrationInput,
  SponsorInput
} from "src/app/graphql/generated/graphql";
import {
  AuthData,
  CardData,
  SecureData,
  AuthorizeResponse
} from "../../core/models";

import { environment } from "../../../environments/environment";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Injectable({
  providedIn: "root"
})
export class RegisterService {
  constructor(
    private processRegistrationGQL: ProcessRegistrationGQL,
    private processSponsorRegistrationGQL: ProcessSponsorRegistrationGQL,
    private formBuilder: FormBuilder
  ) {}

  createSecureData(paymentDetailsForm: FormGroup): SecureData {
    let authData: AuthData = {
      clientKey: environment.clientKey,
      apiLoginID: environment.apiLoginID
    };

    var ccNumber = paymentDetailsForm.controls.cardNumber.value;
    var ccCcv = paymentDetailsForm.controls.cardCode.value;
    var ccDetails = paymentDetailsForm.controls.expirationDate.value.split("/");
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

    return {
      authData: authData,
      cardData: cardData
    };
  }

  sendRegistrationToServer(registration: RegistrationInput): Observable<any> {
    return this.processRegistrationGQL.mutate({
      registration: registration
    });
  }

  sendSponsorRegistrationToServer(sponsor: SponsorInput): Observable<any> {
    return this.processSponsorRegistrationGQL.mutate({
      sponsor : sponsor
    });
  }

  createUserDetailsFormGroup(): FormGroup {
    return this.formBuilder.group({
      firstName: ["", Validators.required],
      lastName: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      organization: ["", Validators.required]
    });
  }

  createAfceaDetailsFormGroup(optional: boolean): FormGroup {
    

    if (optional) {
      return this.formBuilder.group({
        memberId: [
          "",
          [
            Validators.pattern("^[0-9]*$"),
            Validators.minLength(8),
            Validators.maxLength(8)
          ]
        ],
        memberExpireDate: [""],
        isLifeTimeMember: [""]
      });
    }

    return this.formBuilder.group({
      memberId: [
        "",
        [
          Validators.required,
          Validators.pattern("^[0-9]*$"),
          Validators.minLength(8),
          Validators.maxLength(8)
        ]
      ],
      memberExpireDate: ["", Validators.required],
      isLifeTimeMember: [""]
    });
  }

  createPaymentDetailsFormGroup(): FormGroup {
    return this.formBuilder.group({
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

  createNewTuesdayLuncheonRegistration(
    response: AuthorizeResponse,
    userDetailsForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number
  ): RegistrationInput {
    var newLuncheonRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    };

    return newLuncheonRegistration;
  }

  createNewSponsorRegistration(response: AuthorizeResponse,
    userDetailsForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number): SponsorInput {
    var newSponsorRegistration: SponsorInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    }

    return newSponsorRegistration;
  }

  createNewCommitteeRegistration(
    userDetailsForm: FormGroup,
    registrationCodeForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number
  ): RegistrationInput {
    var newCommiteeRegistration: RegistrationInput = {
      dataDescriptor: "",
      dataValue: "",
      registrationCode: registrationCodeForm.controls.registrationCode.value,
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    };

    return newCommiteeRegistration;
  }

  createNewGovernmentRegistration(
    userDetailsForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number,
    afceaDetailsForm: FormGroup
  ): RegistrationInput {
    var newGovernmentRegistration: RegistrationInput = {
      dataDescriptor: "",
      dataValue: "",
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      memberId: afceaDetailsForm.controls.memberId.value || "",
      memberExpirationDate: afceaDetailsForm.controls.memberExpireDate.value.toString() || "",
      isLifeMember: afceaDetailsForm.controls.isLifeTimeMember.value || false,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    };

    return newGovernmentRegistration;
  }

  createNewPaymentAfceanRegistration(
    response: AuthorizeResponse,
    userDetailsForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number,
    afceaDetailsForm: FormGroup
  ): RegistrationInput {
    var newMainRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      memberId: afceaDetailsForm.controls.memberId.value,
      memberExpirationDate: afceaDetailsForm.controls.memberExpireDate.value.toString(),
      isLifeMember: afceaDetailsForm.controls.isLifeTimeMember.value || false,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    };

    return newMainRegistration;
  }

  createNewPaymentRegistration(
    response: AuthorizeResponse,
    userDetailsForm: FormGroup,
    eventRegistrationType: AllEvents.Types,
    mainEventId: number
  ): RegistrationInput {
    var newMainRegistration: RegistrationInput = {
      dataDescriptor: response.opaqueData.dataDescriptor,
      dataValue: response.opaqueData.dataValue,
      firstName: userDetailsForm.controls.firstName.value,
      lastName: userDetailsForm.controls.lastName.value,
      organization: userDetailsForm.controls.organization.value,
      email: userDetailsForm.controls.email.value,
      memberId: "",
      memberExpirationDate: "",
      isLifeMember: false,
      registrationTypeId: eventRegistrationType.registrationTypeId,
      eventId: mainEventId
    };

    return newMainRegistration;
  }
}
