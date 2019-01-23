import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { IntegrationRouting } from './integrations.routing';
import { components, IntegrationMapperNotFoundComponent } from './components';
import { directives } from './directives';
import { pipes } from './pipes';
import { pages } from './pages';
import { CoreModule, ngBootstrapModules } from '../core/core.module';
import { translateSettings } from '../translate.settings';
import { LayoutModule } from '../layout/layout.module';
import { IntegrationTimeregWrapperComponent } from './components/integration-timereg-wrapper/integration-timereg-wrapper.component';
import { IntegrationAltinnWrapperComponent } from './components/integration-altinn-wrapper/integration-altinn-wrapper.component';

@NgModule({
  declarations: [...components, ...directives, ...pipes, ...pages],
  imports: [
    CoreModule.forRoot(),
    LayoutModule,
    CommonModule,
    FormsModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    RouterModule,
    IntegrationRouting,
    ...ngBootstrapModules,
    TranslateModule.forChild(translateSettings),
  ],
  providers: [...pipes],
  entryComponents: [
    IntegrationTimeregWrapperComponent,
    IntegrationAltinnWrapperComponent,
    IntegrationMapperNotFoundComponent,
  ],
})
export class IntegrationsModule {}
