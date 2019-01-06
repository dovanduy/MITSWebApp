import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './admin.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DaysComponent } from './days/days.component';
import { SectionsComponent } from './sections/sections.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { EventsComponent } from './events/events.component';
import { SponsorsComponent } from './sponsors/sponsors.component';
import { CheckinComponent } from './checkin/checkin.component';
import { AdminGuard } from '../core/guards/admin.guard';
import { AuthGuard } from '../core/guards/auth.guard';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { CheckinGuard } from '../core/guards/checkin.guard';
import { LoginComponent } from './login/login.component';

const adminRoutes: Routes = [
    {
        path: '',
        component: AdminComponent,
        children: [
            {
                path: '',
                component: DashboardComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'login',
                component: LoginComponent
            },

            {
                path: 'dashboard',
                component: DashboardComponent,
                canActivate: [AuthGuard]
            },
            {
                path: 'checkin',
                component: CheckinComponent,
                canActivate: [AuthGuard, CheckinGuard]
            },
            {
                path: 'days',
                component: DaysComponent,
                canActivate: [AuthGuard, AdminGuard]
            },
            {
                path: 'sections',
                component: SectionsComponent,
                canActivate: [AuthGuard, AdminGuard]
            },
            {
                path: 'speakers',
                component: SpeakersComponent,
                canActivate: [AuthGuard, AdminGuard]
            },
            {
                path: 'events',
                component: EventsComponent,
                canActivate: [AuthGuard, AdminGuard]
            },
            {
                path: 'sponsors',
                component: SponsorsComponent,
                canActivate: [AuthGuard, AdminGuard]
            },
            
        ]

    }


];

@NgModule({
    imports: [
        RouterModule.forChild(adminRoutes)
    ],
    providers: [
        AuthGuard,
        AdminGuard,
        CheckinGuard
    ],
    exports: [RouterModule]
})
export class AdminRoutingModule { }

