import { Dashboard } from '@sticos/apis/common';

export interface IWidgetInstance {
  widgetId: string;
  widgetName: string;
  settings?: any;
  options: any;
  dragAndDrop?: boolean;
  resizable?: boolean;
  displayName?: string;
  headerName?: string;
  x?: number;
  y?: number;
  xSm?: number;
  ySm?: number;
  xMd?: number;
  yMd?: number;
  xLg?: number;
  yLg?: number;
  xXl?: number;
  yXl?: number;
  w?: number;
  h?: number;
  [propName: string]: any;
}

export interface IDashboardInstance {
  widgets: IWidgetInstance[];
  name: string;
  dashboard: Dashboard;
}
