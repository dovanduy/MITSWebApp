export interface DayInput {
  agendaDay: DateTime;
}

export interface EventInput {
  mainEventId: number;

  isSponsor: boolean;
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

// ====================================================
// END: Apollo Angular template
// ====================================================
