import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AgendaComponent } from './pages/agenda/agenda.component';
import { DetailsComponent } from './pages/details/details.component';
import { AdminModule } from './admin/admin.module';
import { SpeakersComponent } from './pages/speakers/speakers.component';
import { RegisterComponent } from './pages/register/register.component';
import { SponsorsComponent } from './pages/sponsors/sponsors.component';
import { HotelComponent } from "./pages/hotel/hotel.component";     
import { GolfComponent  } from "./pages/golf/golf.component";


const routes: Routes = [
  { path: '', component: DetailsComponent },
  { path: 'agenda', component: AgendaComponent },
  { path: 'admin', loadChildren: './admin/admin.module#AdminModule'},
  { path: '', redirectTo: '', pathMatch: 'full' },
  { path: 'speakers', component: SpeakersComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'sponsors', component: SponsorsComponent},
  { path: 'hotel', component: HotelComponent},
  { path: 'golf', component: GolfComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    //{ enableTracing: true } //TODO: Disable for Production
  )],
  exports: [RouterModule]
})
export class AppRoutingModule { }

