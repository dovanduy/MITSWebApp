import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AdminComponent } from './admin.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const adminRoutes: Routes = [
    {
        path: '',
        component: AdminComponent,
        // Check if role is ISA, spin up ISA Data Context
        //canActivate: [AdminAuthGuardService],
        children: [
            {
                path: 'dashboard',
                component: DashboardComponent
            }
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

