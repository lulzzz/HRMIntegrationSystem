import { Component, OnInit, OnDestroy } from '@angular/core';
import { slideRightLeft } from '@sticos/ui';
import { BehaviorSubject, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import * as _ from 'lodash';
import { ActivatedRoute } from '@angular/router';

import { IWidgetInstance } from '../../models/widget-instance';
import { DashboardService, Dashboard } from '@sticos/apis/common';
import { DashboardGridService } from '../../services';
import { UserCacheService } from 'src/app/core/services';
import { AppConfig } from 'src/app/core/services/app-config-service';

@Component({
  selector: 'app-main-dashboard',
  templateUrl: './main-dashboard.component.html',
  styleUrls: ['./main-dashboard.component.scss'],
  animations: [slideRightLeft],
})
export class MainDashboardComponent implements OnInit, OnDestroy {
  dashboard: Dashboard;
  widgetConfig: IWidgetInstance[];
  title: string;
  id: number;
  subscriptions: any = {};
  isLoading: BehaviorSubject<boolean> = new BehaviorSubject(true);
  customerId: string;
  apiUrls = AppConfig.settings.apiUrls;

  constructor(
    private dashGridService: DashboardGridService,
    private route: ActivatedRoute,
    private dashboardService: DashboardService,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
    this.subscriptions[
      'tittleChange'
    ] = this.dashGridService.onTitleChange().subscribe(data => {
      if (this.id === data.id) {
        this.title = data.newTitle;
        this.dashboard.title = data.newTitle;
      }
    });
  }

  ngOnInit() {
    this.isLoading.next(true);
    this.subscriptions['router'] = this.route.params
      .pipe(
        switchMap(params => {
          this.id = +params['id'];
          return of(this.id);
        }),
        switchMap(id => {
          return this.dashboardService
            .Get({ id, customerId: this.customerId })
            .pipe(
              map(data => ({
                title: data.title,
                widgetConfig: JSON.parse(
                  data.dashboardConfig,
                ) as IWidgetInstance[],
                dashboard: data,
              })),
            );
        }),
      )
      .subscribe(data => {
        this.title = data.title;
        this.widgetConfig = data.widgetConfig;
        this.dashboard = data.dashboard;
        this.isLoading.next(false);
      });
  }

  ngOnDestroy() {
    for (const key in this.subscriptions) {
      if (this.subscriptions.hasOwnProperty(key)) {
        this.subscriptions[key].unsubscribe();
      }
    }
  }

  save(data: IWidgetInstance[]) {
    this.dashboard.dashboardConfig = JSON.stringify(data);
    this.dashboardService
      .Update({
        customerId: this.customerId,
        dashboardResource: this.dashboard,
      })
      .subscribe(() => (this.widgetConfig = data));
  }
}
