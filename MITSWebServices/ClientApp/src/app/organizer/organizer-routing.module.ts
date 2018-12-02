import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';



const organizerRoutes: Routes = [



];

@NgModule({
  imports: [RouterModule.forRoot(organizerRoutes,
    { enableTracing: true } //TODO: Disable for Production
  )],
  exports: [RouterModule]
})
export class AppRoutingModule { }

