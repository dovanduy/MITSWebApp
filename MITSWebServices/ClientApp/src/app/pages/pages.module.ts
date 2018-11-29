import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from './../shared/shared.module';
import { AgendaComponent } from './agenda/agenda.component';
import { DetailsComponent } from './details/details.component';

@NgModule({
  declarations: [AgendaComponent, DetailsComponent],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class PagesModule { }
