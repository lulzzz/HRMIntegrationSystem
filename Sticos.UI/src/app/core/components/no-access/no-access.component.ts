import { Component, OnInit, OnDestroy } from '@angular/core';
import { LayoutService } from '../../../layout/services';

@Component({
  selector: 'app-no-access',
  templateUrl: './no-access.component.html',
  styleUrls: ['./no-access.component.scss'],
})
export class NoAccessComponent implements OnInit, OnDestroy {
  constructor(private layoutService: LayoutService) {}

  ngOnInit() {
    this.layoutService.forceClose();
  }

  ngOnDestroy() {
    this.layoutService.removeForceClose();
  }
}
