import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { CoreModule, ngBootstrapModules } from '../core/core.module';
import { LayoutModule } from '../layout/layout.module';
import { translateSettings } from '../translate.settings';
import { components } from './components';
import { DashboardRouting } from './dashboards.routing';
import { pages } from './pages';
import { SticosWidgetsModule } from '@sticos/widgets';

@NgModule({
  declarations: [...components, ...pages],
  imports: [
    CoreModule.forRoot(),
    SticosWidgetsModule,
    LayoutModule,
    CommonModule,
    FormsModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    RouterModule,
    DashboardRouting,
    ...ngBootstrapModules,
    TranslateModule.forChild(translateSettings),
  ],
})
export class DashboardsModule {}
