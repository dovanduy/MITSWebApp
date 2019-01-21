export interface CheckInAttendeeInput {
  registrationId: number;
}

export interface DayInput {
  agendaDay: DateTime;
}

export interface EventInput {
  mainEventId: number;

  eventRegistrationType: string;
}

export interface SpeakerInput {
  id?: number | null;

  firstName: string;

  lastName: string;

  bio: string;

  title: string;
}

export interface RegistrationInput {
  dataDescriptor: string;

  dataValue: string;

  firstName: string;

  lastName: string;

  organization: string;

  email: string;

  memberId?: string | null;

  memberExpirationDate?: string | null;

  registrationCode?: string | null;

  isLifeMember?: boolean | null;

  isLocal?: boolean | null;

  registrationTypeId: number;

  eventId: number;
}

export interface SponsorInput {
  dataDescriptor: string;

  dataValue: string;

  firstName: string;

  lastName: string;

  organization: string;

  email: string;

  registrationTypeId: number;

  eventId: number;
}

/** The `Date` scalar type represents a year, month and day in accordance with the[ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard. */
export type Date = any;

export type Decimal = any;

/** The `DateTime` scalar type represents a date and time. `DateTime` expectstimestamps to be formatted in accordance with the[ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard. */
export type DateTime = any;

/** The `DateTimeOffset` scalar type represents a date, time and offset from UTC.`DateTimeOffset` expects timestamps to be formatted in accordance with the[ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard. */
export type DateTimeOffset = any;

/** The `Milliseconds` scalar type represents a period of time represented as the total number of milliseconds. */
export type Milliseconds = any;

/** The `Seconds` scalar type represents a period of time represented as the total number of seconds. */
export type Seconds = any;

// ====================================================
// Documents
// ====================================================

export namespace AllEvents {
  export type Variables = {};

  export type Query = {
    __typename?: "Query";

    events: (Events | null)[] | null;
  };

  export type Events = {
    __typename?: "EventType";

    id: number;

    mainEventId: number;

    eventRegistrationType: string;

    waEvent: (WaEvent | null)[] | null;
  };

  export type WaEvent = {
    __typename?: "WaEventType";

    id: number;

    name: string;

    description: string;

    isEnabled: boolean;

    location: string;

    startDate: Date;

    endDate: Date;

    types: (Types | null)[] | null;
  };

  export type Types = {
    __typename?: "WaRegistrationType";

    registrationTypeId: number;

    name: string;

    description: string;

    availableFrom: Date;

    availableThrough: Date;

    basePrice: Decimal;

    codeRequired: boolean;

    isEnabled: boolean;
  };
}

export namespace AllSections {
  export type Variables = {};

  export type Query = {
    __typename?: "Query";

    sections: (Sections | null)[] | null;
  };

  export type Sections = {
    __typename?: "SectionType";

    id: number;

    name: string;

    description: string;

    slideUrl: string;

    restrictSlide: boolean;

    isPanel: boolean;

    startDate: Date;

    endDate: Date;
  };
}

export namespace AllSpeakersSections {
  export type Variables = {};

  export type Query = {
    __typename?: "Query";

    speakers: (Speakers | null)[] | null;
  };

  export type Speakers = {
    __typename?: "SpeakerType";

    id: number;

    firstName: string;

    lastName: string;

    bio: string;

    isPanelist: boolean;

    title: string;

    sections: (Sections | null)[] | null;
  };

  export type Sections = {
    __typename?: "SectionType";

    id: number;

    name: string;
  };
}

export namespace AllSpeakers {
  export type Variables = {};

  export type Query = {
    __typename?: "Query";

    speakers: (Speakers | null)[] | null;
  };

  export type Speakers = {
    __typename?: "SpeakerType";

    id: number;

    firstName: string;

    lastName: string;

    bio: string;

    isPanelist: boolean;

    title: string;
  };
}

export namespace CreateEvent {
  export type Variables = {
    event: EventInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    createEvent: CreateEvent | null;
  };

  export type CreateEvent = {
    __typename?: "EventType";

    id: number;

    mainEventId: number;

    eventRegistrationType: string;
  };
}

export namespace CreateSpeaker {
  export type Variables = {
    speaker: SpeakerInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    createSpeaker: CreateSpeaker | null;
  };

  export type CreateSpeaker = {
    __typename?: "SpeakerType";

    id: number;

    firstName: string;

    lastName: string;

    bio: string;

    title: string;

    sections: (Sections | null)[] | null;
  };

  export type Sections = {
    __typename?: "SectionType";

    id: number;

    name: string;
  };
}

export namespace CheckInAttendee {
  export type Variables = {
    checkInAttendee: CheckInAttendeeInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    checkInAttendee: CheckInAttendee | null;
  };

  export type CheckInAttendee = {
    __typename?: "CheckInAttendeeType";

    checkedIn: boolean;

    status: string;
  };
}

export namespace DeleteSpeaker {
  export type Variables = {
    speakerId: number;
  };

  export type Mutation = {
    __typename?: "Mutation";

    deleteSpeaker: number | null;
  };
}

export namespace ProcessRegistration {
  export type Variables = {
    registration: RegistrationInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    processRegistration: ProcessRegistration | null;
  };

  export type ProcessRegistration = {
    __typename?: "RegistrationType";

    eventRegistrationId: number;

    qrCode: string;
  };
}

export namespace ProcessSponsorRegistration {
  export type Variables = {
    sponsor: SponsorInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    processSponsorRegistration: ProcessSponsorRegistration | null;
  };

  export type ProcessSponsorRegistration = {
    __typename?: "SponsorType";

    eventRegistrationId: number;
  };
}

export namespace UpdateSpeaker {
  export type Variables = {
    speaker: SpeakerInput;
  };

  export type Mutation = {
    __typename?: "Mutation";

    updateSpeaker: UpdateSpeaker | null;
  };

  export type UpdateSpeaker = {
    __typename?: "SpeakerType";

    id: number;

    firstName: string;

    lastName: string;

    bio: string;

    title: string;
  };
}

// ====================================================
// START: Apollo Angular template
// ====================================================

import { Injectable } from "@angular/core";
import * as Apollo from "apollo-angular";

import gql from "graphql-tag";

// ====================================================
// Apollo Services
// ====================================================

@Injectable({
  providedIn: "root"
})
export class AllEventsGQL extends Apollo.Query<
  AllEvents.Query,
  AllEvents.Variables
> {
  document: any = gql`
    query AllEvents {
      events {
        id
        mainEventId
        eventRegistrationType
        waEvent {
          id
          name
          description
          isEnabled
          location
          startDate
          endDate
          types {
            registrationTypeId
            name
            description
            availableFrom
            availableThrough
            basePrice
            codeRequired
            isEnabled
          }
        }
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class AllSectionsGQL extends Apollo.Query<
  AllSections.Query,
  AllSections.Variables
> {
  document: any = gql`
    query AllSections {
      sections {
        id
        name
        description
        slideUrl
        restrictSlide
        isPanel
        startDate
        endDate
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class AllSpeakersSectionsGQL extends Apollo.Query<
  AllSpeakersSections.Query,
  AllSpeakersSections.Variables
> {
  document: any = gql`
    query AllSpeakersSections {
      speakers {
        id
        firstName
        lastName
        bio
        isPanelist
        title
        sections {
          id
          name
        }
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class AllSpeakersGQL extends Apollo.Query<
  AllSpeakers.Query,
  AllSpeakers.Variables
> {
  document: any = gql`
    query AllSpeakers {
      speakers {
        id
        firstName
        lastName
        bio
        isPanelist
        title
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class CreateEventGQL extends Apollo.Mutation<
  CreateEvent.Mutation,
  CreateEvent.Variables
> {
  document: any = gql`
    mutation CreateEvent($event: EventInput!) {
      createEvent(event: $event) {
        id
        mainEventId
        eventRegistrationType
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class CreateSpeakerGQL extends Apollo.Mutation<
  CreateSpeaker.Mutation,
  CreateSpeaker.Variables
> {
  document: any = gql`
    mutation CreateSpeaker($speaker: SpeakerInput!) {
      createSpeaker(speaker: $speaker) {
        id
        firstName
        lastName
        bio
        title
        sections {
          id
          name
        }
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class CheckInAttendeeGQL extends Apollo.Mutation<
  CheckInAttendee.Mutation,
  CheckInAttendee.Variables
> {
  document: any = gql`
    mutation CheckInAttendee($checkInAttendee: CheckInAttendeeInput!) {
      checkInAttendee(checkInAttendee: $checkInAttendee) {
        checkedIn
        status
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class DeleteSpeakerGQL extends Apollo.Mutation<
  DeleteSpeaker.Mutation,
  DeleteSpeaker.Variables
> {
  document: any = gql`
    mutation DeleteSpeaker($speakerId: Int!) {
      deleteSpeaker(speakerId: $speakerId)
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class ProcessRegistrationGQL extends Apollo.Mutation<
  ProcessRegistration.Mutation,
  ProcessRegistration.Variables
> {
  document: any = gql`
    mutation ProcessRegistration($registration: RegistrationInput!) {
      processRegistration(registration: $registration) {
        eventRegistrationId
        qrCode
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class ProcessSponsorRegistrationGQL extends Apollo.Mutation<
  ProcessSponsorRegistration.Mutation,
  ProcessSponsorRegistration.Variables
> {
  document: any = gql`
    mutation ProcessSponsorRegistration($sponsor: SponsorInput!) {
      processSponsorRegistration(sponsor: $sponsor) {
        eventRegistrationId
      }
    }
  `;
}
@Injectable({
  providedIn: "root"
})
export class UpdateSpeakerGQL extends Apollo.Mutation<
  UpdateSpeaker.Mutation,
  UpdateSpeaker.Variables
> {
  document: any = gql`
    mutation UpdateSpeaker($speaker: SpeakerInput!) {
      updateSpeaker(speaker: $speaker) {
        id
        firstName
        lastName
        bio
        title
      }
    }
  `;
}

// ====================================================
// END: Apollo Angular template
// ====================================================
