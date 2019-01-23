import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import * as pages from './pages';

const routes: Routes = [
  {
    path: '', // /admin/news
    component: pages.NewsAdminListComponent,
  },
  {
    path: 'new', // /admin/news/new
    component: pages.NewsAdminEditComponent,
  },
  {
    path: ':id', // /admin/news/1
    component: pages.NewsAdminEditComponent,
  },
];
export const Routing: ModuleWithProviders = RouterModule.forChild(routes);
