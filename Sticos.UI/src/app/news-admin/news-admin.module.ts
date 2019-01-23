import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { translateSettings } from '../translate.settings';
import { pages } from './pages';
import { components } from './components';
import { Routing } from './news-admin.routing';
import { TranslateModule } from '@ngx-translate/core';
import { NewsAdminEditSkeletonComponent } from './pages/edit/skeleton/news-admin-edit-skeleton.component';
import { NewsAdminListSkeletonComponent } from './pages/list/skeleton/news-admin-list-skeleton.component';

@NgModule({
  imports: [CommonModule, Routing, TranslateModule.forChild(translateSettings)],
  declarations: [
    ...pages,
    ...components,
    NewsAdminEditSkeletonComponent,
    NewsAdminListSkeletonComponent,
  ],
})
export class NewsAdminModule {}
