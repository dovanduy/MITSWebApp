import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { SharedModule } from "./../shared/shared.module";
import { ProviderModule } from "../provider/provider.module";

import { AgendaComponent } from "./agenda/agenda.component";
import { DetailsComponent } from "./details/details.component";
import { SpeakersComponent } from "./speakers/speakers.component";
import { SponsorsComponent } from "./sponsors/sponsors.component";
import { RegisterComponent } from "./register/register.component";
import { HeadernavComponent } from "./utility/headernav/headernav.component";
import { FooterComponent } from "./utility/footer/footer.component";
import { HotelComponent } from "./hotel/hotel.component";
import { GolfComponent } from "./golf/golf.component";
import { FaqComponent } from './faq/faq.component';

@NgModule({
  declarations: [
    AgendaComponent,
    DetailsComponent,
    SpeakersComponent,
    SponsorsComponent,
    RegisterComponent,
    HeadernavComponent,
    FooterComponent,
    HotelComponent,
    GolfComponent,
    FaqComponent
  ],
  imports: [CommonModule, SharedModule, RouterModule, ProviderModule],
  exports: [HeadernavComponent, FooterComponent]
})
export class PagesModule {}
