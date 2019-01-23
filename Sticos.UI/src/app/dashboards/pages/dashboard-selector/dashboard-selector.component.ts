import { Component, OnInit, OnDestroy } from '@angular/core';
import { DashboardService } from '@sticos/apis/common';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserCacheService } from 'src/app/core/services';

@Component({
  selector: 'app-dasboard-selector',
  templateUrl: './dashboard-selector.component.html',
})
export class DashboardSelectorComponent implements OnDestroy {
  routerEventsSubscription: Subscription;
  customerId: string;

  constructor(
    private dashboardService: DashboardService,
    private router: Router,
    private userCacheService: UserCacheService,
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
    this.routerEventsSubscription = this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd && this.router.url === '/dashboards') {
        this.dashboardService
          .Search({ customerId: this.customerId })
          .subscribe(data => {
            const defaultDashId = data.find(x => x.isDefault).id;
            this.router.navigateByUrl(`/dashboards/${defaultDashId}`);
          });
      }
    });
  }

  ngOnDestroy() {
    this.routerEventsSubscription.unsubscribe();
  }
}
