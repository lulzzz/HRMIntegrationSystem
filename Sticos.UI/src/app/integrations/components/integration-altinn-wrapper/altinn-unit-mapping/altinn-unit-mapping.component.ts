import { Component, ViewChild, OnInit, Input, Output } from '@angular/core';
import { Alert, EventEmitter } from 'selenium-webdriver';
import { Subject } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { takeUntil } from 'rxjs/operators';
import { UnitWM } from 'src/app/integrations/models/unit-wm';

@Component({
  selector: 'app-altinn-unit-mapping',
  templateUrl: './altinn-unit-mapping.component.html',
  styleUrls: ['./altinn-unit-mapping.component.scss'],
})
export class AltinnUnitMappingComponent implements OnInit {
  onDestroy$: Subject<any> = new Subject();
  _altinnMatchedUnits: UnitWM[] = [];
  config = {};

  // related to devextreme grid
  @ViewChild('altinnNotMatchedGrid')
  altinnNotMatchedGrid;
  // end
  @Input()
  set altinnMatchedUnits(val) {
    this._altinnMatchedUnits = val;
  }
  @Input()
  isLoading: boolean;
  constructor(private translateService: TranslateService) {}

  ngOnInit() {
    this.translateService.onLangChange
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => {
        if (this.altinnNotMatchedGrid) {
          this.altinnNotMatchedGrid.instance.clearSelection();
          this.altinnNotMatchedGrid.instance.refresh();
        }
      });
  }
  // row options
  showOptions(event, id) {
    event.stopPropagation();
    this.config[id] = !this.config[id];
  }
}
