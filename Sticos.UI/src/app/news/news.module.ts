import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { pages } from './pages';
import { components } from './components';
import { Routing } from './news.routing';
import { TranslateModule } from '@ngx-translate/core';
import { translateSettings } from 'src/app/translate.settings';
import { CoreModule } from 'src/app/core/core.module';
import { MomentModule } from 'ngx-moment';
import { LayoutModule } from 'src/app/layout/layout.module';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    LayoutModule,
    RouterModule,
    Routing,
    TranslateModule.forChild(translateSettings),
    CoreModule.forRoot(),
    MomentModule,
  ],
  declarations: [...pages, ...components],
})
export class NewsModule {}
