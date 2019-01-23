import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import * as pages from './pages';

const routes: Routes = [
  {
    path: 'list',
    component: pages.NewsListPageComponent,
  },
  {
    path: ':id',
    component: pages.NewsItemPageComponent,
  },
];
export const Routing: ModuleWithProviders = RouterModule.forChild(routes);
