import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { SharedModule } from './../shared/shared.module';
import { AgendaComponent } from './agenda/agenda.component';
import { DetailsComponent } from './details/details.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { RegisterComponent } from './register/register.component';
import { HeadernavComponent } from './utility/headernav/headernav.component';
import { FooterComponent } from './utility/footer/footer.component';
import { HotelComponent } from './hotel/hotel.component';
import { GolfComponent } from './golf/golf.component';

@NgModule({
  declarations: [AgendaComponent, DetailsComponent, SpeakersComponent, SponsorsComponent, RegisterComponent, HeadernavComponent, FooterComponent, HotelComponent, GolfComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule
  ],
  exports: [
    HeadernavComponent,
  ]
})
export class PagesModule { }
