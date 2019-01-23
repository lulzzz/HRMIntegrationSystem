import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { Ng2EventsModule } from 'ng2-events';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { AppRouting } from './app.routing';

import { ApiModule as CommonApiModule } from '@sticos/apis/common';
import { ApiModule as TimeregApiModule } from '@sticos/apis/timereg';
import { ApiModule as AltinnApiModule } from '@sticos/apis/altinn';
import { ApiModule as IntegrationsApiModule } from '@sticos/apis/integrations';
import { INIT_API_CONFIGURATION } from './app-api-initializer';

import { Ng2CacheModule } from 'ng2-cache';

import { CoreModule, ngBootstrapModules } from './core/core.module';
import { TranslateModule } from '@ngx-translate/core';
import { translateSettings } from './translate.settings';
import { LayoutModule } from './layout/layout.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    AppRouting,
    BrowserModule,
    Ng2EventsModule,
    BrowserAnimationsModule,
    FormsModule,
    ...ngBootstrapModules,
    CommonApiModule,
    TimeregApiModule,
    AltinnApiModule,
    IntegrationsApiModule,
    Ng2CacheModule,
    CoreModule.forRoot(),
    LayoutModule,
    TranslateModule.forRoot(translateSettings),
  ],
  providers: [INIT_API_CONFIGURATION],
  bootstrap: [AppComponent],
  exports: [Ng2EventsModule],
})
export class AppModule {}
