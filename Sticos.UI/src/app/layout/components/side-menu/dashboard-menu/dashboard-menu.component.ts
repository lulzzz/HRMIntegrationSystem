import { Component, OnInit, OnDestroy } from '@angular/core';
import { DashboardService, Dashboard } from '@sticos/apis/common';
import { DashboardMenuItem } from '../../../../dashboards/models/dashboard-menu-item';
import { ActivatedRoute, Router, Event } from '@angular/router';
import { DashboardForm } from '../../../../dashboards/models/dashboard-form';
import { NavigationEnd } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserCacheService } from 'src/app/core/services';
import { DashboardGridService } from 'src/app/dashboards/services';

@Component({
  selector: 'app-dashboard-menu',
  templateUrl: './dashboard-menu.component.html',
  styleUrls: ['../side-menu.component.scss', './dashboard-menu.component.scss'],
})
export class DashboardMenuComponent implements OnInit, OnDestroy {
  private onDestroy$ = new Subject();
  isDashboardCollapsed: boolean;
  isDashboardActive: boolean;
  dahsConfigs: DashboardMenuItem[];
  addDashboardPopupVisible: boolean;
  dashboardForm: DashboardForm;
  customerId: string;

  ngOnInit() {
    this.dashboardService
      .Search({ customerId: this.customerId })
      .subscribe(data => (this.dahsConfigs = data.map(this.mapDashboard)));
  }

  constructor(
    private router: Router,
    private dashboardService: DashboardService,
    private dashboardGridService: DashboardGridService,
    private route: ActivatedRoute,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
    this.router.events
      .pipe(takeUntil(this.onDestroy$))
      .subscribe((event: Event) => {
        if (event instanceof NavigationEnd) {
          this.isDashboardCollapsed =
            this.router.url.indexOf('/dashboards') > -1;
          this.isDashboardActive =
            this.isDashboardActive === undefined
              ? this.router.url.indexOf('/dashboards') > -1
              : this.isDashboardActive;
        }
      });
  }

  startAddingDashboard() {
    this.newDashboard();
    this.addDashboardPopupVisible = true;
  }

  newDashboard() {
    this.dashboardForm = {
      title: `Dashboard ${this.dahsConfigs.length}`,
      selectedTemplate: null,
    };
  }

  addDashboard() {
    const dashboardConfig =
      (this.dashboardForm.selectedTemplate &&
        this.dashboardForm.selectedTemplate.dashboard.dashboardConfig) ||
      '[]';
    this.dashboardService
      .Create({
        customerId: this.customerId,
        dashboardResource: {
          title: this.dashboardForm.title,
          dashboardConfig,
        },
      })
      .subscribe(x => {
        this.dahsConfigs.push(this.mapDashboard(x));
        this.addDashboardPopupVisible = false;
      });
  }

  startEdit(event, data) {
    event.preventDefault();
    event.stopPropagation();
    data.isInEdit = true;
  }

  saveEdit(event, data) {
    event.preventDefault();
    event.stopPropagation();
    if (data.title !== data.newTitle) {
      data.title = data.newTitle;
      data.dashboard.title = data.newTitle;
      this.dashboardService.Update(data.dashboard).subscribe(() => {
        this.dashboardGridService.titleCahnge({
          id: data.id,
          oldTitle: data.title,
          newTitle: data.newTitle,
        });
        data.isInEdit = false;
      });
    }
  }

  discardEdit(event, data) {
    event.preventDefault();
    event.stopPropagation();
    data.newTitle = data.title;
    data.isInEdit = false;
  }

  deleteDashboard(event, data) {
    event.preventDefault();
    event.stopPropagation();
    this.dashboardService.Delete(data.id).subscribe(() => {
      this.dahsConfigs.splice(
        this.dahsConfigs.findIndex(x => x.id === data.id),
        1,
      );
      if (this.router.url === `/dashboards/${data.id}`) {
        this.router.navigate(['/dashboards']);
      }
    });
  }

  private mapDashboard(x: Dashboard): DashboardMenuItem {
    return {
      id: x.id,
      title: x.title,
      link: `/dashboards/${x.id}`,
      isDefault: x.isDefault,
      isInEdit: false,
      newTitle: x.title,
      dashboard: x,
    };
  }

  ngOnDestroy() {
    this.onDestroy$.next();
  }
}
