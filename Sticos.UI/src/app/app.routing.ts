import { ModuleWithProviders } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerAdminGuard } from './core/guards/customer-admin-guard';
import { NotFoundPageComponent, NoAccessComponent } from './core/components';

import * as newsGuards from './news/guards';
import * as newsAdminGuards from './news-admin/guards';

const appRoutes: Routes = [
  {
    path: 'dashboards',
    loadChildren: './dashboards/dashboards.module#DashboardsModule',
  },
  {
    path: 'search',
    loadChildren: './search/search.module#SearchModule',
  },
  {
    path: 'admin',
    children: [
      {
        path: 'integrations',
        canActivate: [CustomerAdminGuard],
        loadChildren: './integrations/integrations.module#IntegrationsModule',
      },
      {
        path: 'news',
        canActivate: [newsAdminGuards.AdminGuard],
        loadChildren: './news-admin/news-admin.module#NewsAdminModule',
      },
    ],
  },
  {
    path: 'news',
    canActivate: [newsGuards.FeatureGuard],
    loadChildren: './news/news.module#NewsModule',
  },
  {
    path: 'page-not-found',
    component: NotFoundPageComponent,
  },
  {
    path: 'no-access',
    component: NoAccessComponent,
  },
  { path: '**', redirectTo: '/page-not-found', pathMatch: 'full' },
  { path: '', redirectTo: '/page-not-found', pathMatch: 'full' },
];

export const AppRouting: ModuleWithProviders = RouterModule.forRoot(appRoutes, {
  enableTracing: false,
});
