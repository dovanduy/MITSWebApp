import {
  ModuleWithProviders, NgModule,
  Optional, SkipSelf,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthService } from './services/auth.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    AuthService,
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only');
    }
  }

  //This makes AuthService a singleton
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        AuthService
      ]
    }
  }
}

