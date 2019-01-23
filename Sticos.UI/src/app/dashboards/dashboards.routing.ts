import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import * as pages from './pages';

const routes: Routes = [
  {
    path: '', // localhost:4200/integration/
    component: pages.DashboardSelectorComponent,
  },
  {
    path: ':id', // localhost:4200/integration/1
    component: pages.MainDashboardComponent,
  },
];
export const DashboardRouting: ModuleWithProviders = RouterModule.forChild(
  routes,
);
