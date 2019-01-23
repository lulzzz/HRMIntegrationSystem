import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MappingCommunicationService {
  unitRefresh: EventEmitter<any> = new EventEmitter();
  employeeRefresh: EventEmitter<any> = new EventEmitter();
  absenceCodeRefresh: EventEmitter<any> = new EventEmitter();
  isConfirmedRows = new BehaviorSubject<boolean>(false);
  constructor() {}

  refreshUnitMapping() {
    this.unitRefresh.emit();
  }

  refreshEmployeeMapping() {
    this.employeeRefresh.emit();
  }

  refreshAbsenceCodeMapping() {
    this.absenceCodeRefresh.emit();
  }
}
