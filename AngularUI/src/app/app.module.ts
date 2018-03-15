import { BrowserModule } from '@angular/platform-browser';
import { HttpModule, XHRBackend, RequestOptions } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app.routing';
import { QueryService } from './services/query.service';
import { HttpInterceptor } from './Helpers/http.interceptor';
import { httpFactory } from './Helpers/httpFactory';
import { CheckinComponent } from './checkin/checkin.component';
import { FinesComponent } from './fines/fines.component';
import { BorrowersComponent } from './borrowers/borrowers.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CheckinComponent,
    FinesComponent,
    BorrowersComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    ModalModule.forRoot(),
    AppRoutingModule
  ],
  providers: [QueryService,
    {
      provide: HttpInterceptor,
      useFactory: httpFactory,
      deps: [XHRBackend, RequestOptions]
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
