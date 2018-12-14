import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';
import { SidenavComponent } from './sidenav/sidenav.component';
import { DaysComponent } from './days/days.component';
import { SectionsComponent } from './sections/sections.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { EventsComponent } from './events/events.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { SpeakersListComponent } from './speakers/list/speakers-list.component';
import { SpeakerEditComponent } from './speakers/edit/speaker-edit.component';
import { SpeakerAddComponent } from './speakers/add/speaker-add.component';

@NgModule({
  declarations: [
    LoginComponent, 
    DashboardComponent, 
    AdminComponent, 
    SidenavComponent, DaysComponent, SectionsComponent, SpeakersComponent, EventsComponent, SponsorsComponent, SpeakersListComponent, SpeakerEditComponent, SpeakerAddComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    RouterModule,
    ReactiveFormsModule,

  ]
})
export class AdminModule { }
