import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EmployeeWM } from '../../models/employee';
import { IntegrationModel } from '../../models/integration';
import { Alert } from '../../../core/models/alert';
import { ExternalDataEnum } from '../../models/entity-map';
import { ConstantsMessage } from '../../shared/constants/message-constants';
import {
  IntegrationService,
  EntityMapService,
  EntityMap,
} from '@sticos/apis/integrations';
import { map, takeUntil } from 'rxjs/operators';
import { ExternalData, ExternalSystemService } from '@sticos/apis/timereg';
import {
  Unit,
  EmployeeService,
  AbsenceTypeService,
  UnitService,
} from '@sticos/apis/common';
import { UnitWM } from '../../models/unit-wm';
import { AbsenceCodeWM } from '../../models/absence-code';
import { forkJoin, Subject } from 'rxjs';
import { State, StateType } from '../../models/state';
import { EnumReaderPipe } from '../../../core/pipes/enum-reader.pipe';
import { TranslateService } from '@ngx-translate/core';
import { MappingCommunicationService } from '../../services';
import { UnitCacheService, UserCacheService } from 'src/app/core/services';
import { AlertService } from 'src/app/core/services/';
import { TimeoutConstants } from 'src/app/core/constants';
import { SticosIntegrationMapper } from '../../shared/sticos-integration-mapper.decorator';
import { IntegrationCategoryEnum } from '../../models/integration-category';

@SticosIntegrationMapper({
  category: IntegrationCategoryEnum.Timereg,
})
@Component({
  selector: 'app-integration-timereg-wrapper',
  templateUrl: './integration-timereg-wrapper.component.html',
  styleUrls: ['./integration-timereg-wrapper.component.scss'],
})
export class IntegrationTimeregWrapperComponent implements OnInit, OnDestroy {
  isEmployeesLoading: boolean;
  isUnitsLoading: boolean;
  isAbsenceCodeLoading: boolean;
  units = [];
  integrationId = 0;
  @Input()
  integration: IntegrationModel;
  alert: Alert;
  externalAbsenceCodes: ExternalData[] = [];
  matchedAbsenceCodes: AbsenceCodeWM[] = [];

  externalEmployees: ExternalData[] = [];
  matchedEmployees: EmployeeWM[] = [];

  externalUnits: ExternalData[] = [];
  matchedUnits: UnitWM[] = [];
  unitIds: number[] = [];

  isLoading = false;
  customerId: string;
  isConfirmedRows: boolean;
  onDestroy$: Subject<any> = new Subject();
  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private integrationEntityService: EntityMapService,
    private unitCacheService: UnitCacheService,
    private unitService: UnitService,
    private integrationService: IntegrationService,
    private externalService: ExternalSystemService,
    private employeeService: EmployeeService,
    private absenceTypeService: AbsenceTypeService,
    private userCacheService: UserCacheService,
    private enumReaderService: EnumReaderPipe,
    private translateService: TranslateService,
    private mappingCommunicationService: MappingCommunicationService,
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
    this.mappingCommunicationService.isConfirmedRows
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(value => {
        this.isConfirmedRows = value;
      });

    this.translateService.onLangChange
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(x => {
        this.mapAbsenceCodeName();
        this.mappingCommunicationService.refreshAbsenceCodeMapping();
      });
  }

  onGetIntegration() {
    this.isLoading = true;
    this.integrationService
      .Get({ customerId: this.customerId, id: this.integrationId })
      .pipe(
        map(x => {
          const i: IntegrationModel = {
            integration: x,
            companyName: '',
          };
          return i;
        }),
      )
      .subscribe(data => {
        this.integration = data;
        this.isLoading = false;
        this.setCompanyName();
        this.onGetUnits();
      });
  }

  findExternalData(
    externalValue,
    externalPropertyName,
    externalEntity,
    externalData,
  ) {
    const externalDataTemp = externalData.filter(x => {
      const filterIdentifiers = x.identifiers.filter(y => {
        if (
          y.value === externalValue &&
          y.property === externalPropertyName &&
          y.entity === externalEntity
        ) {
          return true;
        }
      });
      return filterIdentifiers.length > 0;
    });
    return externalDataTemp[0];
  }

  findCachedExternalData(identifiers, externalData) {
    if (identifiers.length <= 0) {
      return null;
    }

    const externalDataTemp = externalData.filter(x => {
      const filterIdentifiers = x.identifiers.filter(y => {
        if (
          y.value === identifiers[0].value &&
          y.entity === identifiers[0].entity &&
          y.property === identifiers[0].property
        ) {
          return true;
        }
      });
      return filterIdentifiers.length > 0;
    });
    return externalDataTemp[0];
  }

  formatEntitiesToSave(confirmedEntities): EntityMap[] {
    const entitiesToSave = [];
    confirmedEntities.map(x => {
      x.deleted === true
        ? (x.stateInfo.state = State.NoMap)
        : x.ignored === true
          ? (x.stateInfo.state = State.Ignored)
          : (x.stateInfo.state = State.Mapped);

      x.stateInfo.stateType === StateType.S
        ? (x.stateInfo.stateType = StateType.A)
        : (x.stateInfo.stateType = x.stateInfo.stateType);

      x.previousStateInfo.stateType = x.stateInfo.stateType;
      x.previousStateInfo.state = x.stateInfo.state;
      x.previousExternalData = x.entityMatch.externalData;
      if (!x.entityMatch.externalData) {
        const entityToSave: EntityMap = {
          id: 0,
          entityId: x.entityMatch.entityId,
          integrationId: this.integration.integration.id,
          entityName: x.entityMatch.entityMap,
          ignored: x.ignored,
          deleted: x.deleted,
        };

        entitiesToSave.push(entityToSave);
        return;
      }
      const allEntities = x.entityMatch.externalData.identifiers.map(y => {
        const entityToSave: EntityMap = {
          id: 0,
          entityId: x.entityMatch.entityId,
          integrationId: this.integration.integration.id,
          entityName: x.entityMatch.entityMap,
          ignored: x.ignored,
          externalPropertyName: y.property,
          externalValue: y.value,
          externalEntity: y.entity,
        };
        entitiesToSave.push(entityToSave);
      });
    });
    return entitiesToSave;
  }

  saveIntegrationEntities() {
    const confirmedEntities = [
      ...this.matchedAbsenceCodes.filter(
        x => x.stateInfo.state === State.Unsaved,
      ),
      ...this.matchedUnits.filter(x => x.stateInfo.state === State.Unsaved),
      ...this.matchedEmployees.filter(x => x.stateInfo.state === State.Unsaved),
    ];
    const entitiesToSave = this.formatEntitiesToSave(confirmedEntities);
    this.integrationEntityService
      .Update({ customerId: this.customerId, entityMaps: entitiesToSave })
      .subscribe(data => {
        this.mappingCommunicationService.refreshAbsenceCodeMapping();
        this.mappingCommunicationService.refreshUnitMapping();
        this.mappingCommunicationService.refreshEmployeeMapping();
      });

    this.integrationService
      .Update({
        customerId: this.customerId,
        integration: this.integration.integration,
      })
      .subscribe(data => data);

    this.alert = this.alertService.success(
      ConstantsMessage.Integration.Edit.Success,
    );
    setTimeout(() => {
      this.alert = null;
    }, TimeoutConstants.SuccessTimeout);
  }

  setCompanyName() {
    this.unitCacheService
      .GetUnits({ customerId: this.customerId })
      .subscribe(data => {
        this.units = data;
        this.integration.companyName = data.find(
          x => x.id === this.integration.integration.unitId,
        ).name;
      });
  }

  isThereUpdatedEntityMap(localId, entitymap, integrationId, entityMaps) {
    let element = null;
    entityMaps.forEach(el => {
      if (
        localId === el.entityId &&
        entitymap === el.entityName &&
        el.integrationId === integrationId
      ) {
        element = el;
      }
    });
    return element;
  }

  onGetAbsenceCodes() {
    if (this.matchedAbsenceCodes.length > 0) {
      return;
    }
    this.isAbsenceCodeLoading = true;
    const queryData = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.AbsenceCode,
    };

    const queryToUpdated = {
      customerId: this.customerId,
      IntegrationId: this.integrationId,
      EntityName: 'AbsenceType',
    };

    const query = { customerId: this.customerId };

    forkJoin(
      this.externalService.GetExternalData(queryData),
      this.absenceTypeService.Get(query),
      this.integrationEntityService.Get(queryToUpdated),
    ).subscribe(([externalData, absenceCodes, entityMaps]) => {
      this.externalAbsenceCodes = externalData;
      absenceCodes.map(y => {
        let externalDataTemp = null;
        let state = State.NoMap;
        const entityMap = this.isThereUpdatedEntityMap(
          parseInt(y.value, 10),
          'AbsenceType',
          this.integrationId,
          entityMaps,
        );
        if (entityMap !== null) {
          externalDataTemp = this.findExternalData(
            entityMap.externalValue,
            entityMap.externalPropertyName,
            entityMap.externalEntity,
            externalData,
          );
          entityMap.ignored ? (state = State.Ignored) : (state = State.Mapped);
        }

        const matchedAbsenceCode = {
          entityMap: 'AbsenceType',
          entityId: parseInt(y.value, 10),
          externalData: externalDataTemp,
        };
        const m: AbsenceCodeWM = {
          entityMapId: parseInt(y.value, 10),
          entityMatch: matchedAbsenceCode,
          absenceCode: y,
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
          autoMap: false,
          deleted: false,
          absenceCodeName: this.enumReaderService.transform(
            y.value,
            'AbsenceType',
          ),
        };
        this.matchedAbsenceCodes.push(m);
        this.isAbsenceCodeLoading = false;
      });
    });
  }

  tryMatchAbsenceCode(val) {
    const query = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.AbsenceCode,
      action: 'Try match entity',
      ids: val,
    };
    let numberOfMatched = 0;
    this.externalService.MatchEntities(query).subscribe(x => {
      x.map(y => {
        const absenceCode = this.matchedAbsenceCodes.find(
          a => a.entityMapId === y.entityId,
        );

        absenceCode.deleted = false;

        if (y.externalData !== null) {
          const externalData = this.findCachedExternalData(
            y.externalData.identifiers,
            this.externalAbsenceCodes,
          );
          absenceCode.entityMatch.externalData = externalData;
          absenceCode.stateInfo.stateType = StateType.S;
          absenceCode.autoMap = true;
          absenceCode.stateInfo.state = State.Suggestion;
          numberOfMatched++;
        }
      });
      this.mappingCommunicationService.refreshAbsenceCodeMapping();
      let msg = `${numberOfMatched} `;
      this.translateService
        .get('integrations.mapping.common.automatic-match-warning')
        .subscribe(result => {
          msg += result;
        });

      if (numberOfMatched) {
        this.translateService
          .get('integrations.mapping.common.automatic-match-found-warning')
          .subscribe(result => {
            msg += ' ' + result;
          });
      }

      this.alert = this.alertService.warning(msg);
      setTimeout(() => {
        this.alert = null;
      }, TimeoutConstants.WarningTimeout);
    });
  }

  mapAbsenceCodeName() {
    this.matchedAbsenceCodes.map(x => {
      x.absenceCodeName = this.enumReaderService.transform(
        x.absenceCode.value,
        'AbsenceType',
      );
    });
  }

  onGetUnits() {
    if (this.matchedUnits.length > 0) {
      return;
    }
    this.isUnitsLoading = true;
    const queryData = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.Unit,
    };

    const queryToUpdated = {
      customerId: this.customerId,
      IntegrationId: this.integrationId,
      EntityName: 'Unit',
    };
    forkJoin(
      this.externalService.GetExternalData(queryData),
      this.unitService.GetHierarchyDown({
        customerId: this.customerId,
        id: this.integration.integration.unitId,
      }),
      this.unitService.GetUnit({
        customerId: this.customerId,
        id: this.integration.integration.unitId,
      }),
      this.integrationEntityService.Get(queryToUpdated),
    ).subscribe(([externalData, unitsHierarchy, unit, entityMaps]) => {
      this.externalUnits = externalData;
      this.units = unitsHierarchy;
      this.unitIds = unitsHierarchy.map(x => x.id);

      let externalDataTemp = null;
      let state = State.NoMap;

      const entityMap = this.isThereUpdatedEntityMap(
        unit.id,
        'Unit',
        this.integrationId,
        entityMaps,
      );

      if (entityMap !== null) {
        externalDataTemp = this.findExternalData(
          entityMap.externalValue,
          entityMap.externalPropertyName,
          entityMap.externalEntity,
          externalData,
        );
        entityMap.ignored ? (state = State.Ignored) : (state = State.Mapped);
      }

      const matchedUnit = {
        entityMap: 'Unit',
        entityId: unit.id,
        externalData: externalDataTemp,
      };

      const m: UnitWM = {
        entityMapId: unit.id,
        entityMatch: matchedUnit,
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
        previousExternalData: matchedUnit.externalData,
        autoMap: false,
        deleted: false,
      };
      this.matchedUnits.push(m);
      this.isUnitsLoading = false;
    });
  }

  tryMatchUnits(val) {
    const query = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.Unit,
      action: 'Try match entity',
      ids: val,
    };
    let numberOfMatched = 0;
    this.externalService.MatchEntities(query).subscribe(x => {
      x.map(y => {
        const unit = this.matchedUnits.find(a => a.entityMapId === y.entityId);
        unit.deleted = false;

        if (y.externalData !== null) {
          const externalData = this.findCachedExternalData(
            y.externalData.identifiers,
            this.externalUnits,
          );
          unit.entityMatch.externalData = externalData;
          unit.stateInfo.stateType = StateType.S;
          unit.stateInfo.state = State.Suggestion;
          unit.autoMap = true;
          numberOfMatched++;
        }
      });
      this.mappingCommunicationService.refreshUnitMapping();
      let msg = `${numberOfMatched} `;
      this.translateService
        .get('integrations.mapping.common.automatic-match-warning')
        .subscribe(result => {
          msg += result;
        });

      if (numberOfMatched) {
        this.translateService
          .get('integrations.mapping.common.automatic-match-found-warning')
          .subscribe(result => {
            msg += ' ' + result;
          });
      }

      this.alert = this.alertService.warning(msg);
      setTimeout(() => {
        this.alert = null;
      }, TimeoutConstants.WarningTimeout);
    });
  }

  onGetEmployees() {
    if (this.matchedEmployees.length > 0) {
      return;
    }
    this.isEmployeesLoading = true;
    const queryData = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.Employee,
    };

    const queryToUpdated = {
      customerId: this.customerId,
      IntegrationId: this.integrationId,
      EntityName: 'Employee',
    };
    const query = {
      customerId: this.customerId,
      Take: 2500,
      UnitIds: this.unitIds,
    };
    forkJoin(
      this.externalService.GetExternalData(queryData),
      this.employeeService.Search(query),
      this.integrationEntityService.Get(queryToUpdated),
    ).subscribe(([externalData, employees, entityMaps]) => {
      this.externalEmployees = externalData;
      employees.map(y => {
        let externalDataTemp = null;
        let state = State.NoMap;

        const entityMap = this.isThereUpdatedEntityMap(
          y.id,
          'Employee',
          this.integrationId,
          entityMaps,
        );

        if (entityMap !== null) {
          externalDataTemp = this.findExternalData(
            entityMap.externalValue,
            entityMap.externalPropertyName,
            entityMap.externalEntity,
            externalData,
          );
          entityMap.ignored ? (state = State.Ignored) : (state = State.Mapped);
        }

        const matchedEmployee = {
          entityMap: 'Employee',
          entityId: y.id,
          externalData: externalDataTemp,
        };

        const m: EmployeeWM = {
          entityMapId: y.id,
          entityMatch: matchedEmployee,
          employee: y,
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
          previousExternalData: matchedEmployee.externalData,
          autoMap: false,
          deleted: false,
          unitName: this.units.find(x => x.id === y.unitId).name,
        };
        this.matchedEmployees.push(m);
        this.isEmployeesLoading = false;
      });
    });
  }

  tryMatchEmployees(val) {
    const query = {
      customerId: this.customerId,
      id: this.integration.integration.externalSystem.valueOf(),
      unitId: this.integration.integration.unitId.valueOf(),
      entity: ExternalDataEnum.Employee,
      action: 'Try match entity',
      ids: val,
    };
    let numberOfMatched = 0;
    this.externalService.MatchEntities(query).subscribe(x => {
      x.map(y => {
        const employee = this.matchedEmployees.find(
          a => a.entityMapId === y.entityId,
        );
        employee.deleted = false;

        if (y.externalData !== null && y.externalData.validForUse) {
          const externalData = this.findCachedExternalData(
            y.externalData.identifiers,
            this.externalEmployees,
          );
          employee.entityMatch.externalData = externalData;
          employee.stateInfo.stateType = StateType.S;
          employee.stateInfo.state = State.Suggestion;
          employee.autoMap = true;
          numberOfMatched++;
        }
      });
      this.mappingCommunicationService.refreshEmployeeMapping();
      let msg = `${numberOfMatched} `;
      this.translateService
        .get('integrations.mapping.common.automatic-match-warning')
        .subscribe(result => {
          msg += result;
        });

      if (numberOfMatched) {
        this.translateService
          .get('integrations.mapping.common.automatic-match-found-warning')
          .subscribe(result => {
            msg += ' ' + result;
          });
      }

      this.alert = this.alertService.warning(msg);
      setTimeout(() => {
        this.alert = null;
      }, TimeoutConstants.WarningTimeout);
    });
  }

  ngOnDestroy() {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }
}
