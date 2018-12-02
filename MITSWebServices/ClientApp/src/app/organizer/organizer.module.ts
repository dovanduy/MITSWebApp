import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from './../shared/shared.module';
import { OrganizerComponent } from './organizer.component';

@NgModule({
  declarations: [OrganizerComponent],
  imports: [
    CommonModule,
    SharedModule,
  ]
})
export class OrganizerModule { }
