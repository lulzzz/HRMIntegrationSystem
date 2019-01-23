import { NewsAdminEditComponent } from './edit/news-admin-edit.component';
import { NewsAdminListComponent } from './list/news-admin-list.component';
import { NewsAdminEditSkeletonComponent } from './edit/skeleton/news-admin-edit-skeleton.component';
import { NewsAdminListSkeletonComponent } from './list/skeleton/news-admin-list-skeleton.component';

export * from './edit/news-admin-edit.component';
export * from './list/news-admin-list.component';
export * from './edit/skeleton/news-admin-edit-skeleton.component';
export * from './list/skeleton/news-admin-list-skeleton.component';

export const pages = [
  NewsAdminEditComponent,
  NewsAdminListComponent,
  NewsAdminEditSkeletonComponent,
  NewsAdminListSkeletonComponent,
];
