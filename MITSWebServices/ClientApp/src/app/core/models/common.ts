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
