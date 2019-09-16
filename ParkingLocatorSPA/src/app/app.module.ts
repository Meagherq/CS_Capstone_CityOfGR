import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbCardModule, NbIconModule, NbActionsModule, NbSearchModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { MainMapComponent } from './main-map/main-map.component';

@NgModule({
   declarations: [
      AppComponent,
      HomeComponent,
      MainMapComponent
   ],
   imports: [
    BrowserModule,
    NoopAnimationsModule,
    NbThemeModule.forRoot({ name: 'default' }),
    NbLayoutModule,
    NbEvaIconsModule,
    AppRoutingModule,
    HttpClientModule,
    NbCardModule,
    NbIconModule,
    NbActionsModule,
    NbSearchModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
