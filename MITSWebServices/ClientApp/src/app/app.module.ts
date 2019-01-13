import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule, MatButtonModule, ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PagesModule } from './pages/pages.module';
import { OrganizerModule } from './organizer/organizer.module';
import { CoreModule } from './core/core.module';
import { GraphQLModule } from './graphql/graphql.module';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    PagesModule,
    OrganizerModule,
    AppRoutingModule,
    MatToolbarModule,
    FlexLayoutModule,
    MatButtonModule,
    GraphQLModule,
    HttpClientModule,
    CoreModule.forRoot(),
   
  ],
  providers: [{ provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }],
  bootstrap: [AppComponent]
})
export class AppModule { }
