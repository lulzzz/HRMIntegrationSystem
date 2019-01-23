import { Component, OnInit, Input } from '@angular/core';
import { IEmployee } from '@sticos/apis/common';

@Component({
  selector: 'app-altinn-employee-mapping',
  templateUrl: './altinn-employee-mapping.component.html',
  styleUrls: ['./altinn-employee-mapping.component.scss'],
})
export class AltinnEmployeeMappingComponent implements OnInit {
  @Input()
  employees: IEmployee;
  constructor() {}

  ngOnInit() {}
}
