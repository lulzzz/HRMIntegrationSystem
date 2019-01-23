import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild,
  OnDestroy,
} from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs/index';
import { ExternalData } from '@sticos/apis/timereg';

import DataSource from 'devextreme/data/data_source';
import { AbsenceCodeWM } from 'src/app/integrations/models/absence-code';
import { Alert } from 'selenium-webdriver';
import { TranslateService } from '@ngx-translate/core';
import { MappingCommunicationService } from 'src/app/integrations/services';
import { State, StateType } from 'src/app/integrations/models/state';
@Component({
  selector: 'app-timereg-absence-type-mapping',
  templateUrl: './timereg-absence-type-mapping.component.html',
  styleUrls: ['./timereg-absence-type-mapping.component.scss'],
})
export class TimeregAbsenceTypeMappingComponent implements OnInit, OnDestroy {
  _matchedUAbsenceCodes: AbsenceCodeWM[] = [];
  alert: Alert;
  selectedItems = true;
  needToBeConfirmed = 0;
  config = {};
  onDestroy$: Subject<any> = new Subject();
  dataSource: DataSource;
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

  alertmessage = false;

  @Input()
  set externalAbsenceCodes(val) {
    this.dataSource = new DataSource({
      store: val,
      group: 'identifiers[0].entity',
    });
  }
  @Input()
  integrationId: number;
  @Input()
  isLoading: boolean;
  @Output()
  matchedAbsenceCodesChange = new EventEmitter();

  //  event emmiter try match
  @Output()
  tryMatchChange = new EventEmitter();
  mappedRows: boolean;

  @Input()
  set matchedAbsenceCodes(val) {
    this._matchedUAbsenceCodes = val;
    this.matchedAbsenceCodesChange.emit(this._matchedUAbsenceCodes);
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
    this.mappingCommunicationService.absenceCodeRefresh
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => {
        this.checkNeedConfirmedRows();
        if (this.notMatchedGrid) {
          this.notMatchedGrid.instance.refresh();
        }
      });
  }

  loadStateFilters() {
    this.stateFilter.forEach(state => {
      this.translateService.get(state.key).subscribe(x => {
        state.text = x;
      });
    });
  }

  ngOnDestroy() {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }

  calculateFilterExpression(filterValue, selectedOperation) {
    const column = this as any;

    if (selectedOperation === 'contains') {
      const filterExpression = [[column.dataField, 'contains', filterValue]];
      return filterExpression;
    }
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

  get matchedAbsenceCodesMatches() {
    return this._matchedUAbsenceCodes;
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
      x.stateInfo.stateType = StateType.I;
      x.deleted = false;
    });
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  onDeleteMatch() {
    let confirmed = this.notMatchedGrid.instance.getSelectedRowsData();
    confirmed = confirmed.filter(
      x =>
        x.stateInfo.state === State.Mapped ||
        x.stateInfo.state === State.Ignored,
    );

    confirmed.map(x => {
      x.ignored = false;
      x.entityMatch.externalData = null;
      x.stateInfo.state = State.Unsaved;
      x.stateInfo.stateType = StateType.R;
      x.deleted = true;
    });
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
  }

  tryMatchAbsenceCode() {
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
    currentRow.data.entityMatch.externalData = e.value;
    if (e.value !== null && currentRow.data.autoMap === false) {
      currentRow.data.stateInfo.stateType = StateType.M;
      currentRow.data.stateInfo.state = State.Unsaved;
      this.notMatchedGrid.instance.refresh();
    }
    currentRow.data.autoMap = false;
  }

  searchItems(row) {
    const castedRow = row as ExternalData;
    const values = castedRow.dataSet.map(x => x.value).join(', ');
    return values;
  }

  customStateSort(rowData) {
    return rowData.stateInfo.state;
  }

  tryMatchSingleAbsenceCode(id) {
    const array = [id];
    this.tryMatchChange.emit(array);
    this.config[id] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  confirmSingleAbsenceCode(data) {
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

  ignoreSingleAbsenceCode(data) {
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

  deleteSingleAbsenceCode(data) {
    const currentUnit = data.data;

    if (
      currentUnit.stateInfo.state === State.Mapped ||
      currentUnit.stateInfo.state === State.Ignored
    ) {
      currentUnit.ignored = false;
      currentUnit.entityMatch.externalData = null;
      currentUnit.stateInfo.state = State.Unsaved;
      currentUnit.stateInfo.stateType = StateType.R;
      currentUnit.deleted = true;
    }

    this.config[currentUnit.entityMapId] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
  }

  resetSingleAbsenceCode(data) {
    const currentAbsenceCode = data.data;
    currentAbsenceCode.autoMap = true;
    currentAbsenceCode.stateInfo.state =
      currentAbsenceCode.previousStateInfo.state;
    currentAbsenceCode.stateInfo.stateType =
      currentAbsenceCode.previousStateInfo.stateType;
    this.config[currentAbsenceCode.entityMapId] = false;
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  // row options
  showOptions(event, id) {
    event.stopPropagation();
    this.config[id] = !this.config[id];
  }

  checkNeedConfirmedRows() {
    this.needToBeConfirmed = this._matchedUAbsenceCodes.filter(
      x =>
        x.stateInfo.stateType === StateType.S &&
        x.stateInfo.state !== State.Unsaved,
    ).length;
    this.mappingCommunicationService.isConfirmedRows.next(
      this.needToBeConfirmed > 0,
    );
  }
}
