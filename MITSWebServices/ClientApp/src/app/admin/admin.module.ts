import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';

import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AdminComponent } from './admin.component';
import { AdminRoutingModule } from './admin-routing.module';
import { SidenavComponent } from './sidenav/sidenav.component';

@NgModule({
  declarations: [
    LoginComponent, 
    DashboardComponent, 
    AdminComponent, 
    SidenavComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,

  ]
})
export class AdminModule { }
