import { Component, OnInit, OnDestroy } from '@angular/core';
import { LayoutService } from '../../../layout/services';

@Component({
  selector: 'app-not-found-page',
  templateUrl: './not-found-page.component.html',
  styleUrls: ['./not-found-page.component.scss'],
})
export class NotFoundPageComponent implements OnInit, OnDestroy {
  constructor(private layoutService: LayoutService) {}

  ngOnInit() {
    this.layoutService.forceClose();
  }

  ngOnDestroy() {
    this.layoutService.removeForceClose();
  }
}
