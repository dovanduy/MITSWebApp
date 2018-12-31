import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';

import { RegisterDialogComponent } from '../../provider/register-dialog/register-dialog.component';
import { ProviderService } from '../../provider/provider.service';
import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private allEventsGQL: AllEventsGQL, private provider: ProviderService ) { }

  events: AllEvents.Events[];
  //mainEvent: AllEvents.Events;

  ngOnInit() {
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      console.log(this.events);
      this.activate();

    });


  }

  activate() {
    this.events = this.events.filter(event => event.isSponsor != true);
  }

  register(type: AllEvents.Types, eventId: number) {
   this.provider.openRegisterDialog(type, eventId);
  }


}
