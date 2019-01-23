import { Component, OnInit } from '@angular/core';
import { IntegrationCategory } from '../../models/integration-category';
import { IntegrationCategoryService } from '../../services/integration-category.service';
import { IntegrationService, Integration } from '@sticos/apis/integrations';
import { IntegrationModel } from '../../models/integration';
import { Unit } from '@sticos/apis/common';
import { forkJoin } from 'rxjs';
import { ViewChild } from '@angular/core';
import { DxDataGridComponent } from 'devextreme-angular';
import { UnitCacheService, UserCacheService } from 'src/app/core/services';

@Component({
  selector: 'app-integrations-list',
  templateUrl: './integrations-list.component.html',
  styleUrls: ['./integrations-list.component.scss'],
})
export class IntegrationsListComponent implements OnInit {
  isDeletingIntegration: boolean;
  integrationForDelete: Integration;
  deactiveIntegration: Integration;
  showDeleteAlert: boolean;
  isLoading: boolean;
  units: Unit[];
  categories: IntegrationCategory[];
  integrations: IntegrationModel[] = [];
  config = {};
  customerId: any;

  @ViewChild('dataGrid')
  gridComponent: DxDataGridComponent;
  showDeactivateAlert: boolean;
  isDeactivatedIntegration: boolean;
  isUpdating: boolean;

  constructor(
    private integrationService: IntegrationService,
    private unitCacheService: UnitCacheService,
    private categoryService: IntegrationCategoryService,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
  }

  ngOnInit() {
    this.onGetIntegrations();
  }

  onGetIntegrations() {
    this.isLoading = true;
    const query = {
      customerId: this.customerId,
    };

    forkJoin(
      this.integrationService.Search(query),
      this.unitCacheService.GetUnits({ customerId: this.customerId }),
    ).subscribe(([integrations, units]) => {
      integrations.map(x => {
        const i: IntegrationModel = {
          integration: x,
          companyName: '',
        };
        this.integrations.push(i);
      });
      this.units = units;
      this.setCompaniesName();
      this.onGetCategories();
    });
  }
  setCompaniesName() {
    this.integrations.forEach(el => {
      el.companyName = this.units.find(
        x => el.integration.unitId === x.id,
      ).name;
    });
  }

  onGetCompanies() {
    this.unitCacheService
      .GetUnits({ customerId: this.customerId })
      .subscribe(data => {
        this.units = data;
        this.setCompaniesName();
      });
  }

  onGetCategories() {
    this.categoryService.getCategories().subscribe(data => {
      this.categories = data;
      this.isLoading = false;
    });
  }

  onRowPrepared(e) {
    if (e.rowType === 'data') {
      if (!e.data.integration.isActivated) {
        e.rowElement.bgColor = '#fae8bf';
      }
    }
  }

  showOptions(event, id) {
    event.stopPropagation();
    this.config[id] = !this.config[id];
  }

  showDeleteDialog(event, integration: Integration) {
    event.stopPropagation();
    this.config[integration.id] = false;
    this.showDeleteAlert = true;
    this.integrationForDelete = integration;
  }

  deleteIntegration() {
    this.isDeletingIntegration = true;
    this.integrationService
      .Delete({ id: this.integrationForDelete.id, customerId: this.customerId })
      .subscribe(() => {
        const index = this.integrations.findIndex(
          x => x.integration.id === this.integrationForDelete.id,
        );
        this.integrations.splice(index, 1);
        this.gridComponent.instance.refresh();
        this.isDeletingIntegration = false;
        this.hideDeleteDialog();
      });
  }

  cancelDeleteIntegration() {
    this.hideDeleteDialog();
  }

  hideDeleteDialog() {
    this.integrationForDelete = null;
    this.showDeleteAlert = false;
  }
  checkIntegrationStatus(event, integration: Integration) {
    event.stopPropagation();
    this.config[integration.id] = !this.config[integration.id];
    this.deactiveIntegration = integration;
    if (integration.isActivated) {
      this.showDeactivateAlert = true;
    } else {
      this.changeStatusIntegration();
    }
  }
  cancelDeactivateIntegration() {
    this.hideDeactivateDialog();
  }

  hideDeactivateDialog() {
    this.deactiveIntegration = null;
    this.showDeactivateAlert = false;
  }

  changeStatusIntegration() {
    this.isUpdating = true;
    this.deactiveIntegration.isActivated = !this.deactiveIntegration
      .isActivated;
    this.integrationService
      .Update({
        customerId: this.customerId,
        integration: this.deactiveIntegration,
      })
      .subscribe(() => {
        this.isUpdating = false;
        this.hideDeactivateDialog();
      });
  }
}
