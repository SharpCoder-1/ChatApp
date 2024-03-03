import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { PlayComponent } from './play/play.component';
import {HTTP_INTERCEPTORS,provideHttpClient,withInterceptors} from '@angular/common/http';
import { jwtInterceptor } from './shared/interceptors/jwt.interceptor';
import {  } from 'ng2-charts';
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    PlayComponent],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule,
    SharedModule    
  ],
  providers: [provideHttpClient(withInterceptors([jwtInterceptor]))],
  bootstrap: [AppComponent]
})
export class AppModule { }
