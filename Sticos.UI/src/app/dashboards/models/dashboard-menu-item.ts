import { Dashboard } from '@sticos/apis/common';

export class DashboardMenuItem {
  id: number;
  title: string;
  link: string;
  isDefault: boolean;
  isInEdit: boolean;
  newTitle: string;
  dashboard: Dashboard;
}
