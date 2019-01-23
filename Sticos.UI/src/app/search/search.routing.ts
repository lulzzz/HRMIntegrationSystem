import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import * as pages from './pages';

const routes: Routes = [
  {
    path: '', // /search
    component: pages.SearchPageComponent,
  },
];
export const Routing: ModuleWithProviders = RouterModule.forChild(routes);
