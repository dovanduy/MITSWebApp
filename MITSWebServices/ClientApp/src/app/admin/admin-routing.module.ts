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

const adminRoutes: Routes = [
    {
        path: '',
        component: AdminComponent,
        children: [
            {
                path: 'dashboard',
                component: DashboardComponent
            },
            {
                path: 'checkin',
                component: CheckinComponent
            },
            {
                path: 'days',
                component: DaysComponent
            },
            {
                path: 'sections',
                component: SectionsComponent
            },
            {
                path: 'speakers',
                component: SpeakersComponent
            },
            {
                path: 'events',
                component: EventsComponent
            },
            {
                path: 'sponsors',
                component: SponsorsComponent
            },
            
        ]

    }


];

@NgModule({
    imports: [
        RouterModule.forChild(adminRoutes)
    ],
    exports: [RouterModule]
})
export class AdminRoutingModule { }

