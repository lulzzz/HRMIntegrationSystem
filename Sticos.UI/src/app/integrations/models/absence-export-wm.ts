import { AbsenceExport } from '@sticos/apis/timereg';
import { Unit, IEmployee } from '@sticos/apis/common';
import { AbsenceEntry } from './absence-entry';

export class AbsenceExportWM {
  absenceExport: AbsenceExport;
  absenceEntries: AbsenceEntry[];
  employee: IEmployee;
  unit: Unit;
}
