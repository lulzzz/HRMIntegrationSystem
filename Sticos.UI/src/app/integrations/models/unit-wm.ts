import { Unit } from '@sticos/apis/common';
import { EntityMatch, ExternalData } from '@sticos/apis/timereg';
import { State, StateInfo } from './state';

export class UnitWM {
  entityMapId: number;
  unit: Unit;
  entityMatch: EntityMatch;
  confirm: boolean;
  ignored: boolean;
  deleted: boolean;
  stateInfo: StateInfo;
  previousStateInfo: StateInfo;
  previousExternalData: ExternalData;
  autoMap: boolean;
}
