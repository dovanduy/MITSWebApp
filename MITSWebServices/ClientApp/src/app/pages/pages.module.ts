import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from './../shared/shared.module';
import { AgendaComponent } from './agenda/agenda.component';
import { DetailsComponent } from './details/details.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { RegisterComponent } from './register/register.component';
import { HeadernavComponent } from './utility/headernav/headernav.component';

@NgModule({
  declarations: [AgendaComponent, DetailsComponent, SpeakersComponent, SponsorsComponent, RegisterComponent, HeadernavComponent],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports: [
    HeadernavComponent,
  ]
})
export class PagesModule { }
