import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

import { AllEvents } from "src/app/graphql/generated/graphql";

@Injectable({
  providedIn: "root"
})
export class GlobalService {
  constructor() {}

  // events$: BehaviorSubject<AllEvents.Events[]> = new BehaviorSubject({} as AllEvents.Events[]);

  // events(events: AllEvents.Events[]): void {
  //   this.events$.next(events);
  // }
}
