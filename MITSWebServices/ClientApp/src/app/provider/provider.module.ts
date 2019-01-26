import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";

import { SharedModule } from "../shared/shared.module";
import { RegisterDialogComponent } from "./register-dialog/register-dialog.component";
import { CommitteeRegisterDialogComponent } from "./committee-register-dialog/committee-register-dialog.component";
import { GovernmentRegisterDialogComponent } from "./government-register-dialog/government-register-dialog.component";
import { IndustryRegisterDialogComponent } from "./industry-register-dialog/industry-register-dialog.component";
import { RegisterDialogService } from "./services/register-dialog.service";
import { RegisterService } from "./services/register.service";
import { SponsorRegisterDialogComponent } from './sponsor-register-dialog/sponsor-register-dialog.component';
import { ShowticketsComponent } from './showtickets/showtickets.component';


@NgModule({
  declarations: [
    RegisterDialogComponent,
    CommitteeRegisterDialogComponent,
    GovernmentRegisterDialogComponent,
    IndustryRegisterDialogComponent,
    SponsorRegisterDialogComponent,
    ShowticketsComponent
  ],
  imports: [SharedModule, ReactiveFormsModule],
  providers: [RegisterDialogService, RegisterService],
  entryComponents: [
    RegisterDialogComponent,
    CommitteeRegisterDialogComponent,
    GovernmentRegisterDialogComponent,
    IndustryRegisterDialogComponent,
    SponsorRegisterDialogComponent
  ]
})
export class ProviderModule {}
