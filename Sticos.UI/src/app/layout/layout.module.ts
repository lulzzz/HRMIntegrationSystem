import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { CoreModule, ngBootstrapModules } from '../core/core.module';
import { exportedComponents, components } from './components';
import { LayoutService } from './services';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    CoreModule.forRoot(),
    ...ngBootstrapModules,
    FormsModule,
    TranslateModule,
  ],
  declarations: [...components],
  exports: [exportedComponents],
  providers: [LayoutService],
})
export class LayoutModule {}
