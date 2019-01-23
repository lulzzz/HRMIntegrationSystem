import { IEmployee } from '@sticos/apis/common';
import { EntityMatch, ExternalData } from '@sticos/apis/timereg';
import { State, StateInfo } from './state';

export class EmployeeWM {
  entityMapId: number;
  employee: IEmployee;
  entityMatch: EntityMatch;
  confirm: boolean;
  ignored: boolean;
  deleted: boolean;
  stateInfo: StateInfo;
  previousStateInfo: StateInfo;
  previousExternalData: ExternalData;
  autoMap: boolean;
  unitName: string;
}
