import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ViewChild,
  Input,
} from '@angular/core';
import { ExternalData } from '@sticos/apis/timereg';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { OnDestroy } from '@angular/core';
import { takeUntil } from 'rxjs/internal/operators/takeUntil';
import { EmployeeWM } from 'src/app/integrations/models/employee';
import { Alert } from 'selenium-webdriver';
import { MappingCommunicationService } from 'src/app/integrations/services';
import { State, StateType } from 'src/app/integrations/models/state';

@Component({
  selector: 'app-timereg-employee-mapping',
  templateUrl: './timereg-employee-mapping.component.html',
  styleUrls: ['./timereg-employee-mapping.component.scss'],
})
export class TimeregEmployeeMappingComponent implements OnInit, OnDestroy {
  _matchedEmployees: EmployeeWM[] = [];
  alert: Alert;
  selectedItems = true;
  needToBeConfirmed = 0;
  config = {};
  onDestroy$: Subject<any> = new Subject();

  stateFilter = [
    {
      text: 'Mapped',
      key: 'integrations.mapping.common.mapped',
      value: 1,
    },
    {
      text: 'Ignored',
      key: 'integrations.mapping.common.ignored',
      value: 2,
    },
    {
      text: 'Nomap',
      key: 'integrations.mapping.common.nomap',
      value: 4,
    },
    {
      text: 'Unsaved',
      key: 'integrations.mapping.common.unsaved',
      value: 3,
    },
  ];

  // related to devextreme grid
  @ViewChild('notMatchedGrid')
  notMatchedGrid;

  // end

  alertmessage = false;

  @Input()
  externalEmployees: ExternalData[];
  @Input()
  integrationId: number;
  @Input()
  isLoading: boolean;
  @Output()
  matchedEmployeesChange = new EventEmitter();

  //  event emmiter try match
  @Output()
  tryMatchChange = new EventEmitter();
  mappedRows: boolean;

  @Input()
  set matchedEmployees(val) {
    this._matchedEmployees = val;
    this.matchedEmployeesChange.emit(this._matchedEmployees);
  }

  constructor(
    private translateService: TranslateService,
    private mappingCommunicationService: MappingCommunicationService,
  ) {
    this.handleFilterState = this.handleFilterState.bind(this);
  }

  ngOnInit() {
    this.translateService.onLangChange
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => {
        if (this.notMatchedGrid) {
          this.notMatchedGrid.instance.refresh();
        }
      });
    this.loadStateFilters();
    this.mappingCommunicationService.employeeRefresh
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => {
        this.checkNeedConfirmedRows();
        if (this.notMatchedGrid) {
          this.notMatchedGrid.instance.refresh();
        }
      });
  }

  ngOnDestroy() {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  calculateCellValue(rowData) {
    return rowData.employee.firstName + ' ' + rowData.employee.lastName;
  }

  loadStateFilters() {
    this.stateFilter.forEach(state => {
      this.translateService.get(state.key).subscribe(x => {
        state.text = x;
      });
    });
  }

  handleFilterState(filterValue, selectedFilterOperation) {
    return [
      this.getStateFiltering,
      selectedFilterOperation || 'contains',
      filterValue,
    ];
  }

  private getStateFiltering(rowData) {
    let statusString: string;
    statusString = rowData.stateInfo.state;
    return statusString;
  }

  get matchedEmployees() {
    return this._matchedEmployees;
  }

  onSubmitMatch() {
    let confirmed = this.notMatchedGrid.instance.getSelectedRowsData();
    confirmed = confirmed.filter(
      x =>
        typeof x.entityMatch.externalData === 'object' &&
        x.entityMatch.externalData !== null,
    );

    confirmed.map(x => {
      x.stateInfo.state = State.Unsaved;
      x.ignored = false;
      x.deleted = false;
    });
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  onIgnoreMatch() {
    const confirmed = this.notMatchedGrid.instance.getSelectedRowsData();
    confirmed.map(x => {
      x.ignored = true;
      x.entityMatch.externalData = null;
      x.stateInfo.state = State.Unsaved;
      x.stateInfo.stateTyPE = StateType.I;
      x.deleted = false;
    });
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  onDeleteMatch() {
    const confirmed = this.notMatchedGrid.instance.getSelectedRowsData();

    confirmed.map(x => {
      x.ignored = false;
      x.entityMatch.externalData = null;
      x.stateInfo.state = State.Unsaved;
      x.stateInfo.stateType = StateType.R;
      x.deleted = true;
    });
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  tryMatchAEmployees() {
    const ids = this.notMatchedGrid.instance
      .getSelectedRowsData()
      .map(x => x.entityMapId);
    this.notMatchedGrid.instance.clearSelection();
    this.tryMatchChange.emit(ids);
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  onSelectionChanged() {
    const selectedRows = this.notMatchedGrid.instance.getSelectedRowsData();
    selectedRows.length > 0
      ? (this.selectedItems = false)
      : (this.selectedItems = true);
    this.mappedRows =
      selectedRows.filter(
        y =>
          y.stateInfo.state === State.Mapped ||
          y.stateInfo.state === State.Ignored,
      ).length > 0;
  }

  onRowPrepared(e) {
    if (e.rowType === 'data') {
      if (e.data.stateInfo.state === State.NoMap) {
        e.rowElement.bgColor = '#f4c0cd';
      } else if (e.data.stateInfo.state === State.Suggestion) {
        e.rowElement.bgColor = '#ffc04d';
      } else if (e.data.stateInfo.state === State.Unsaved) {
        e.rowElement.bgColor = '#fae8bf';
      } else if (
        e.data.stateInfo.state === State.Ignored ||
        e.data.stateInfo.state === State.Mapped
      ) {
        e.rowElement.bgColor = '#c3e2ce';
      }
    }
  }

  onValueChanged(e, currentRow) {
    if (!e.value || e.value === currentRow.data.entityMatch.externalData) {
      return;
    }
    if (!e.value.validForUse) {
      e.component.option('value', e.previousValue);
      return;
    }
    currentRow.data.entityMatch.externalData = e.value;
    if (e.value !== null && currentRow.data.autoMap === false) {
      currentRow.data.stateInfo.stateType = StateType.M;
      currentRow.data.stateInfo.state = State.Unsaved;
      this.notMatchedGrid.instance.refresh();
    }
    currentRow.data.autoMap = false;
  }

  searchItems(row) {
    const custedRow = row as ExternalData;
    const values = custedRow.dataSet.map(x => x.value).join(', ');
    return values;
  }

  customNameSort(rowData) {
    return rowData.employee.firstName;
  }

  customStateSort(rowData) {
    return rowData.stateInfo.state;
  }

  tryMatchSingleEmployee(id) {
    const array = [id];
    this.tryMatchChange.emit(array);
    this.config[id] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  confirmSingleEmployee(data) {
    const currentUnit = data.data;

    if (currentUnit.entityMatch.externalData !== null) {
      currentUnit.stateInfo.state = State.Unsaved;
      currentUnit.ignored = false;
      currentUnit.deleted = false;
    }
    this.config[currentUnit.entityMapId] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  ignoreSingleEmployee(data) {
    const currentUnit = data.data;

    currentUnit.ignored = true;
    currentUnit.entityMatch.externalData = null;
    currentUnit.stateInfo.state = State.Unsaved;
    currentUnit.stateInfo.stateType = StateType.I;
    currentUnit.deleted = false;

    this.config[currentUnit.entityMapId] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  deleteSingleEmployee(data) {
    const currentEmployee = data.data;

    if (
      currentEmployee.stateInfo.state === State.Mapped ||
      currentEmployee.stateInfo.state === State.Ignored
    ) {
      currentEmployee.ignored = false;
      currentEmployee.entityMatch.externalData = null;
      currentEmployee.stateInfo.state = State.Unsaved;
      currentEmployee.stateInfo.stateType = StateType.R;
      currentEmployee.deleted = true;
    }

    this.config[currentEmployee.entityMapId] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
  }

  resetSingleEmployee(data) {
    const currentEmployee = data.data;
    currentEmployee.autoMap = true;
    currentEmployee.stateInfo.state = currentEmployee.previousStateInfo.state;
    currentEmployee.stateInfo.stateType =
      currentEmployee.previousStateInfo.stateType;
    currentEmployee.entityMatch.externalData =
      currentEmployee.previousExternalData;
    this.config[currentEmployee.entityMapId] = false;
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  // row options
  showOptions(event, id) {
    event.stopPropagation();
    this.config[id] = !this.config[id];
  }

  checkNeedConfirmedRows() {
    this.needToBeConfirmed = this._matchedEmployees.filter(
      x =>
        x.stateInfo.stateType === StateType.S &&
        x.stateInfo.state !== State.Unsaved,
    ).length;
    this.mappingCommunicationService.isConfirmedRows.next(
      this.needToBeConfirmed > 0,
    );
  }
}
