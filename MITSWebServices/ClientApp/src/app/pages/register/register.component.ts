import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';

import { RegisterDialogComponent } from '../../provider/register-dialog/register-dialog.component';
import { ProviderService } from '../../provider/provider.service';
import { AllEventsGQL, AllEvents } from 'src/app/graphql/generated/graphql';
import { JwtHelperService } from 'src/app/core/services/jwt-helper.service';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private allEventsGQL: AllEventsGQL, private provider: ProviderService, private jwtHelper: JwtHelperService, private authService: AuthService ) { }

  events: AllEvents.Events[];
  //mainEvent: AllEvents.Events;

  ngOnInit() {
    this.allEventsGQL.watch().valueChanges.subscribe(result => {
      this.events = result.data.events;
      console.log(this.events);
      this.activate();

    });

    var token = this.authService.getJwt();
    var decodedToken = this.jwtHelper.decodeToken(token);

    console.log(decodedToken);


  }

  activate() {
    this.events = this.events.filter(event => event.eventRegistrationType == "Main");
  }

  register(type: AllEvents.Types, eventId: number) {
   this.provider.openRegisterDialog(type, eventId);
  }


}
