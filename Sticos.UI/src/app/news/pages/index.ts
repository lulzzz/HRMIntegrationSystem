import { NewsListPageComponent } from './list/news-list-page.component';
import { NewsItemPageComponent } from './item/news-item-page.component';
import { NewsListPageSkeletonComponent } from './list/skeleton/news-list-page-skeleton.component';
import { NewsItemPageSkeletonComponent } from './item/skeleton/news-item-page-skeleton.component';

export * from './list/news-list-page.component';
export * from './item/news-item-page.component';
export * from './list/skeleton/news-list-page-skeleton.component';
export * from './item/skeleton/news-item-page-skeleton.component';

export const pages = [
  NewsListPageComponent,
  NewsItemPageComponent,
  NewsListPageSkeletonComponent,
  NewsItemPageSkeletonComponent,
];
