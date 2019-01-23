import { HeaderComponent } from './header/header.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { SticosProductSwitcherComponent } from './header/sticos-product-switcher/sticos-product-switcher.component';
import { UserDropdownComponent } from './header/user-dropdown/user-dropdown.component';
import { UserNotificationsComponent } from './header/user-notifications/user-notifications.component';
import { AdminMenuComponent } from './side-menu/admin-menu/admin-menu.component';
import { DashboardMenuComponent } from './side-menu/dashboard-menu/dashboard-menu.component';
import { SpinnerComponent } from './header/spinner/spinner.component';
import { GlobalSearchComponent } from './header/search/global-search.component';

export const exportedComponents = [HeaderComponent, SideMenuComponent];

export const components = [
  ...exportedComponents,
  SpinnerComponent,
  SticosProductSwitcherComponent,
  UserDropdownComponent,
  UserNotificationsComponent,
  AdminMenuComponent,
  DashboardMenuComponent,
  GlobalSearchComponent,
];
