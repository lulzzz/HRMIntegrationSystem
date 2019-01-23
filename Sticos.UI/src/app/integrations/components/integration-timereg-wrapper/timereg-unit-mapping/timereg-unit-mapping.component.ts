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
import { ExternalData } from '@sticos/apis/timereg';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs/internal/Subject';
import { UnitWM } from 'src/app/integrations/models/unit-wm';
import { Alert } from 'selenium-webdriver';
import { MappingCommunicationService } from 'src/app/integrations/services';
import { State, StateType } from 'src/app/integrations/models/state';

@Component({
  selector: 'app-timereg-unit-mapping',
  templateUrl: './timereg-unit-mapping.component.html',
  styleUrls: ['./timereg-unit-mapping.component.scss'],
})
export class TimeregUnitMappingComponent implements OnInit, OnDestroy {
  _matchedUnits: UnitWM[] = [];
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
  externalUnits: ExternalData[];
  @Input()
  integrationId: number;
  @Input()
  isLoading: boolean;

  @Output()
  matchedUnitsChange = new EventEmitter();

  //  event emmiter try match
  @Output()
  tryMatchChange = new EventEmitter();
  mappedRows: boolean;

  @Input()
  set matchedUnits(val) {
    this._matchedUnits = val;
    this.matchedUnitsChange.emit(this._matchedUnits);
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
          this.notMatchedGrid.instance.clearSelection();
          this.notMatchedGrid.instance.refresh();
        }
      });

    this.loadStateFilters();
    this.mappingCommunicationService.unitRefresh
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

  calculateCellValue(rowData) {
    return rowData.unit.name;
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

  get matchedUnits() {
    return this._matchedUnits;
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

  tryMatchUnits() {
    const ids = this.notMatchedGrid.instance
      .getSelectedRowsData()
      .map(x => x.entityMapId);
    this.tryMatchChange.emit(ids);
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
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

  tryMatchSingleUnit(id) {
    const array = [id];
    this.tryMatchChange.emit(array);
    this.config[id] = false;
    this.notMatchedGrid.instance.clearSelection();
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  confirmSingleUnit(data) {
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

  ignoreSingleUnit(data) {
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

  deleteSingleUnit(data) {
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

  resetSingleUnit(data) {
    const currentUnit = data.data;
    currentUnit.autoMap = true;
    currentUnit.stateInfo.state = currentUnit.previousStateInfo.state;
    currentUnit.stateInfo.stateType = currentUnit.previousStateInfo.stateType;
    currentUnit.entityMatch.externalData = currentUnit.previousExternalData;
    this.config[currentUnit.entityMapId] = false;
    this.notMatchedGrid.instance.refresh();
    this.checkNeedConfirmedRows();
  }

  // row options
  showOptions(event, id) {
    event.stopPropagation();
    this.config[id] = !this.config[id];
  }

  checkNeedConfirmedRows() {
    this.needToBeConfirmed = this._matchedUnits.filter(
      x =>
        x.stateInfo.stateType === StateType.S &&
        x.stateInfo.state !== State.Unsaved,
    ).length;
    this.mappingCommunicationService.isConfirmedRows.next(
      this.needToBeConfirmed > 0,
    );
  }
}
