import { Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminDataService {

  pageTitle$: BehaviorSubject<String> = new BehaviorSubject("");

  pageTitle(pageTitle: string): void {
    this.pageTitle$.next(pageTitle);
  }
}
