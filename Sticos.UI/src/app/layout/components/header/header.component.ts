import { Component, OnInit, OnDestroy } from '@angular/core';
import { LayoutService } from '../../services/layout.service';
import { slideRightLeft } from '@sticos/ui';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { LoaderSpinnerService, EventService } from 'src/app/core/services';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [slideRightLeft],
})
export class HeaderComponent implements OnInit, OnDestroy {
  collapse = false;
  isLoading: boolean;
  onDestroy$: Subject<any> = new Subject();

  constructor(
    private layoutService: LayoutService,
    private loaderSpinnerService: LoaderSpinnerService,
    private eventService: EventService,
  ) {}

  ngOnInit() {
    this.loaderSpinnerService.isLoading
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(value => {
        this.isLoading = value;
      });

    this.eventService.globalSearch
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => {
        this.collapse = !this.collapse;
      });
  }

  toggleSideMenu() {
    this.layoutService.sidebarToggle();
  }

  ngOnDestroy() {
    this.onDestroy$.next();
    this.onDestroy$.complete();
  }
}
