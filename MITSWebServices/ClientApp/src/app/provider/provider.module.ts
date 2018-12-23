import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from "@angular/forms";

import { SharedModule } from "../shared/shared.module";
import { RegisterDialogComponent } from './register-dialog/register-dialog.component';


@NgModule({
  declarations: [
    RegisterDialogComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [

  ],
  entryComponents: [RegisterDialogComponent]
})
export class ProviderModule { }
