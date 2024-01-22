import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from './shared/components/layout/layout.module';
import { LoginModule } from './shared/components/login/login.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { interceptorInterceptor } from './shared/interceptors/interceptor.interceptor';




@NgModule({
  declarations: [
    AppComponent,
 

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    LayoutModule,
    LoginModule
    
  
  ],
 providers: [{provide: HTTP_INTERCEPTORS, useClass: interceptorInterceptor, multi: true } ],
  bootstrap: [AppComponent]
})
export class AppModule { }
