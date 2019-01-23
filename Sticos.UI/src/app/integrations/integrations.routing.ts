import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import * as pages from './pages';
import { CustomerResolver } from '../core/guards/customer.resolver';
import { IntegrationRouterComponent } from './pages/integration-router/integration-router.component';

const routes: Routes = [
  {
    path: '',
    component: pages.IntegrationsListComponent,
    resolve: {
      customer: CustomerResolver,
    },
  },
  {
    path: 'new',
    component: pages.IntegrationAddComponent,
  },
  {
    path: ':id',
    component: IntegrationRouterComponent,
    resolve: {
      customer: CustomerResolver,
    },
  },

  {
    path: 'absence/:id',
    component: pages.AbsenseExportComponent,
  },
];
export const IntegrationRouting: ModuleWithProviders = RouterModule.forChild(
  routes,
);
