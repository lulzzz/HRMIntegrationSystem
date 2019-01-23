import { ICode } from '@sticos/apis/integrations';
import { EntityMatch } from '@sticos/apis/timereg';
import { State, StateType, StateInfo } from './state';

export class AbsenceCodeWM {
  entityMapId: number;
  absenceCode: ICode;
  entityMatch: EntityMatch;
  confirm: boolean;
  ignored: boolean;
  deleted: boolean;
  stateInfo: StateInfo;
  previousStateInfo: StateInfo;
  absenceCodeName: string;
  autoMap: boolean;
}

export enum AbsenceType {
  NotSet = 0,
  SelfdeclarationSick = 1000,
  SelfdeclarationChildminderSick = 2000,
  SelfdeclarationChildSick = 3000,
  Sickleave = 4000,
  SickleaveChild = 5000,
  Vacation = 6000,
  Timeoff = 7000,
  SocialLeave = 8000,
  ParentalLeave = 9000,
  CareLeave = 10000,
  EducationalLeave = 11000,
  NursingLeave = 12000,
  CaregivingLeave = 13000,
  MilitaryLeave = 14000,
  OtherLeave = 15000,
  CourseTravel = 16000,
  MeetingTravel = 17000,
  CustomervisitTravel = 18000,
  OtherTravel = 19000,
  Invalid = 20000,
}
