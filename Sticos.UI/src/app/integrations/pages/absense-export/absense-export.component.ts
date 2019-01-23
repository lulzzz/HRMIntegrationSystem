import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IntegrationService } from '@sticos/apis/integrations';
import { AbsenceExportService } from '@sticos/apis/timereg';
import { forkJoin } from 'rxjs';
import { EmployeeService, UnitService } from '@sticos/apis/common';
import { AbsenceExportWM } from '../../models/absence-export-wm';
import { UnitCacheService, UserCacheService } from 'src/app/core/services';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-absense-export',
  templateUrl: './absense-export.component.html',
  styleUrls: ['./absense-export.component.scss'],
})
export class AbsenseExportComponent implements OnInit {
  absenceExports: AbsenceExportWM[] = [];

  integrationId = 0;
  currentIntegration = null;
  customerId: string;
  units = [];
  constructor(
    private absenceExportService: AbsenceExportService,
    private route: ActivatedRoute,
    private integrationService: IntegrationService,
    private unitCacheService: UnitCacheService,
    private employeeService: EmployeeService,
    private userCacheService: UserCacheService,
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
    this.onGetData();
  }

  onGetData() {
    this.integrationService
      .Get({ customerId: this.customerId, id: this.integrationId })
      .pipe(
        switchMap(x => {
          this.currentIntegration = x;
          return this.unitService.GetHierarchyDown({
            customerId: this.customerId,
            id: x.unitId,
          });
        }),
      )
      .subscribe(x => {
        const query = {
          UnitId: this.currentIntegration.unitId,
          customerId: this.customerId,
        };
        this.units = x;
        this.onGetAbsenceExports(query);
      });
  }

  onGetAbsenceExports(query: AbsenceExportService.GetAllParams) {
    this.absenceExportService.GetAll(query).subscribe(x => {
      this.employeeService
        .Search({
          customerId: this.customerId,
          EmployeesIds: Array.from(new Set(x.map(a => a.employeeId))),
        })
        .subscribe(employees => {
          x.map(ae => {
            const absenceJson = JSON.parse(ae.absenceJson);
            const absenceLength = absenceJson.AbsenceEntries.length;
            const absenceExportWm: AbsenceExportWM = {
              absenceExport: ae,
              absenceEntries: absenceJson.AbsenceEntries,
              employee: employees.find(e => e.id === ae.employeeId),
              unit: this.units.find(u => u.id === ae.unitId),
            };
            this.absenceExports.push(absenceExportWm);
          });
        });
    });
  }
  exportAbsence(absenceId, e) {
    e.stopPropagation();
    const queryResend = {
      customerId: this.customerId,
      absenceExportId: absenceId,
      action: 'resend',
    };

    this.absenceExportService.Execute(queryResend).subscribe(() => {
      const query = {
        UnitId: this.currentIntegration.unitId,
        customerId: this.customerId,
      };
      this.absenceExports = [];
      this.onGetAbsenceExports(query);
    });
  }

  customLastChangedSort(rowData) {
    if (rowData.absenceExport.updatedBy) {
      return rowData.absenceExport.updateAt;
    } else {
      return rowData.absenceExport.createdAt;
    }
  }

  onRowClicked(e) {
    const key = e.component.getKeyByRowIndex(e.rowIndex);
    const expanded = e.component.isRowExpanded(key);
    expanded ? e.component.collapseRow(key) : e.component.expandRow(key);
  }
}
