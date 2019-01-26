import { AllEvents } from "src/app/graphql/generated/graphql";

export interface AuthorizeResponse {
  messages: AuthorizeMessages;
  opaqueData: AuthorizeOpaqueData;
}

export interface AuthorizeMessages {
  message: AuthorizeMessage[];
  resultCode: string;
}

export interface AuthorizeMessage {
  code: string,
  text: string
}

export interface AuthorizeOpaqueData {
  dataDescriptor: string;
  dataValue: string;
}

export interface SecureData {
  cardData: CardData;
  authData: AuthData;
}

export interface CardData {
  cardNumber: string;
  month: string;
  year: string;
  cardCode: string;
}

export interface AuthData {
  clientKey: string;
  apiLoginID: string;
}

export interface GraphQLProcessSponsorResponse {
  data: SponsorData;
  errors: GraphQlError[]
}

export interface GraphQLProcessRegistrationResponse {
  data: RegistrationData;
  errors: GraphQlError[]
}

export interface RegistrationData {
  processRegistration: ProcessRegistration
}

export interface SponsorData {
  processSponsorRegistration: ProcessSponsorRegistration
}

export interface ProcessSponsorRegistration {
  eventRegistrationId: number;
}

export interface ProcessRegistration {
  qrCode: string;
  eventRegistrationId: number;
}

export interface GraphQlError {
  locations: any[];
  message: string;
  path: any[]
}

export interface Ticket {
  eventRegistrationId: number;
  qrCode: string;
  event: AllEvents.Types;
}
