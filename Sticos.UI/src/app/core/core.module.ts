import { NgModule, ErrorHandler, ModuleWithProviders } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { pipes } from './pipes';
import { components } from './components';
import { LoaderInterceptor } from './loader.interceptor';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { JL } from 'jsnlog';
import { UncaughtExceptionHandler } from './error.handler';

import {
  DxPopupModule,
  DxDataGridModule,
  DxAutocompleteModule,
  DxLookupModule,
  DxTooltipModule,
  DxTemplateModule,
  DxRadioGroupModule,
} from 'devextreme-angular';
import { translateSettings } from '../translate.settings';
import { NgbAlertModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {
  TabsModule,
  AlertModule,
  BsDropdownModule,
  PopoverModule,
} from 'ngx-bootstrap';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { GridsterModule } from 'angular2gridster';
import { SticosUiModule } from '@sticos/ui';
import { SticosWidgetsModule } from '@sticos/widgets';

const dxImports = [
  DxPopupModule,
  DxDataGridModule,
  DxAutocompleteModule,
  DxLookupModule,
  DxTooltipModule,
  DxTemplateModule,
  DxRadioGroupModule,
];

export const ngBootstrapModules = [
  NgbAlertModule.forRoot(),
  TabsModule.forRoot(),
  AlertModule.forRoot(),
  BsDropdownModule.forRoot(),
  NgbModule.forRoot(),
  PopoverModule.forRoot(),
  NgIdleKeepaliveModule.forRoot(),
  GridsterModule.forRoot(),
  BsDropdownModule.forRoot(),
];

@NgModule({
  imports: [
    SticosWidgetsModule,
    CommonModule,
    RouterModule,
    HttpClientModule,
    SticosUiModule,
    TranslateModule.forChild(translateSettings),
    ...ngBootstrapModules,
  ],
  declarations: [...pipes, ...components],
  exports: [
    ...pipes,
    ...components,
    ...dxImports,
    HttpClientModule,
    SticosUiModule,
  ],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        {
          provide: HTTP_INTERCEPTORS,
          useClass: LoaderInterceptor,
          multi: true,
        },
        { provide: 'JSNLOG', useValue: JL },
        { provide: ErrorHandler, useClass: UncaughtExceptionHandler },
        ...pipes,
      ],
    };
  }
}
