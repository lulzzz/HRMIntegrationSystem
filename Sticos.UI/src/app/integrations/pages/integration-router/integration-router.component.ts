import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import {
  IntegrationService,
  IntegrationCategoryService,
  Integration,
  ICode,
} from '@sticos/apis/integrations';
import { UserCacheService, UnitCacheService } from 'src/app/core/services';
import { IntegrationModel } from '../../models/integration';

@Component({
  selector: 'app-integration-router',
  templateUrl: './integration-router.component.html',
})
export class IntegrationRouterComponent implements OnInit, OnDestroy {
  categories: ICode[];
  isLoading = true;
  category: string;
  integration: IntegrationModel = new IntegrationModel();
  id: number;
  subscriptions: any = {};
  customerId: string;

  constructor(
    private router: ActivatedRoute,
    private integrationService: IntegrationService,
    private integrationCategoryService: IntegrationCategoryService,
    private userCacheService: UserCacheService,
    private unitCacheService: UnitCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.subscriptions['router'] = this.router.params
      .pipe(
        switchMap(params => {
          this.id = +params.id;
          const observables = forkJoin([
            this.integrationService.Get({
              id: this.id,
              customerId: this.customerId,
            }),
            this.integrationCategoryService.Get(this.customerId),
          ]);
          return observables;
        }),
      )
      .subscribe(responses => {
        this.integration.integration = responses[0];
        this.categories = responses[1];
        this.category = this.categories.find(
          x => +x.value === this.integration.integration.category,
        ).value;
        this.setCompanyName();
        this.isLoading = false;
      });
  }

  setCompanyName() {
    this.unitCacheService
      .GetUnits({ customerId: this.customerId })
      .subscribe(data => {
        this.integration.companyName = data.find(
          x => x.id === this.integration.integration.unitId,
        ).name;
      });
  }

  ngOnDestroy(): void {
    for (const key in this.subscriptions) {
      if (this.subscriptions.hasOwnProperty(key)) {
        this.subscriptions[key].unsubscribe();
      }
    }
  }
}
