import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AgendaComponent } from './pages/agenda/agenda.component';
import { DetailsComponent } from './pages/details/details.component';


const routes: Routes = [
  { path: '', component: DetailsComponent },
  { path: 'agenda', component: AgendaComponent }


];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    { enableTracing: true } //TODO: Disable for Production
  )],
  exports: [RouterModule]
})
export class AppRoutingModule { }

