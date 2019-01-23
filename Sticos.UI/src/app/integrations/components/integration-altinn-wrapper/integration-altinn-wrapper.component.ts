import { Component, OnInit, Input } from '@angular/core';
import { SticosIntegrationMapper } from '../../shared/sticos-integration-mapper.decorator';
import { IntegrationCategoryEnum } from '../../models/integration-category';
import { UserCacheService, UnitCacheService } from 'src/app/core/services';
import { ActivatedRoute } from '@angular/router';
import { IntegrationModel } from '../../models/integration';
import { UnitWM } from '../../models/unit-wm';
import { StateType, State } from '../../models/state';
import { forkJoin } from 'rxjs';
import { ExternalSystemService } from '@sticos/apis/altinn';
import { ExternalData } from '@sticos/apis/timereg';
import { EmployeeService, IEmployee, UnitService } from '@sticos/apis/common';

@SticosIntegrationMapper({
  category: IntegrationCategoryEnum.Goverment,
})
@Component({
  selector: 'app-integration-altinn-wrapper',
  templateUrl: './integration-altinn-wrapper.component.html',
  styleUrls: ['./integration-altinn-wrapper.component.scss'],
})
export class IntegrationAltinnWrapperComponent implements OnInit {
  customerId: any;
  integrationId: number;
  @Input()
  integration: IntegrationModel;
  altinnMatchedUnits: UnitWM[] = [];
  employees: IEmployee[] = [];
  units: any;
  currentUnit: UnitWM;
  altinnExternalUnits: ExternalData[];
  unitIds: number[] = [];
  constructor(
    private route: ActivatedRoute,
    private userCacheService: UserCacheService,
    private unitCacheService: UnitCacheService,
    private employeeService: EmployeeService,
    private externalService: ExternalSystemService,
    private unitService: UnitService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
    if (this.route.snapshot.params['id']) {
      this.integrationId = +this.route.snapshot.params['id'];
    }
  }

  ngOnInit() {
    this.onGetUnits();
  }

  onGetUnits() {
    if (this.altinnMatchedUnits.length > 0) {
      return;
    }

    const queryData = {
      id: this.integration.integration.externalSystem,
      customerId: this.customerId,
    };

    forkJoin(
      this.externalService.GetExternalData(queryData),
      this.unitService.GetHierarchyDown({
        customerId: this.customerId,
        id: this.integration.integration.unitId,
      }),
      this.unitCacheService.GetUnits({ customerId: this.customerId }),
    ).subscribe(([externalData, unitHierarchy, units]) => {
      this.altinnExternalUnits = externalData;
      this.unitIds = unitHierarchy.map(x => x.id);
      const unit = units.find(
        u => u.id === this.integration.integration.unitId,
      );
      const state = State.NoMap;

      const m: UnitWM = {
        entityMapId: unit.id,
        entityMatch: null,
        unit: unit,
        confirm: false,
        ignored: false,
        stateInfo: {
          state: state,
          stateType: StateType.Unknown,
        },
        previousStateInfo: {
          state: state,
          stateType: StateType.Unknown,
        },
        previousExternalData: null,
        autoMap: false,
        deleted: false,
      };
      this.altinnMatchedUnits.push(m);
    });
  }

  onGetEmployees() {
    const query = {
      customerId: this.customerId,
      UnitIds: this.unitIds,
      Take: 2500,
    };
    this.employeeService.Search(query).subscribe(x => {
      this.employees = x;
    });
  }
}
