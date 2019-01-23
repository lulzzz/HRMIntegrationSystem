import { Component, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { LayoutService } from './layout/services/layout.service';
import { TranslateService } from '@ngx-translate/core';
import { Subject, Observable } from 'rxjs';
import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { takeUntil } from 'rxjs/operators';

import { UserSettingsService } from './core/services';
import { locale, loadMessages } from 'devextreme/localization';
import 'devextreme-intl';

declare var require: any;
const messagesNo = require('../assets/i18n/devextreme.no.json');
loadMessages(messagesNo);
import { AppConfig } from './core/services/app-config-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnDestroy {
  pingUrl = AppConfig.settings.pingUrl;
  countdownMessage$: Observable<string>;
  isIdle: boolean;
  sideMenuOpen: boolean;
  onDestroy$ = new Subject();

  constructor(
    private layoutService: LayoutService,
    private translate: TranslateService,
    private userSettingsService: UserSettingsService,
    private idle: Idle,
    private keepAlive: Keepalive,
    private changeDetectionRef: ChangeDetectorRef,
  ) {
    this.sideMenuOpen = this.layoutService.isOpen;
    this.layoutService.sidebarToggled
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(isOpen => (this.sideMenuOpen = isOpen));

    let language = userSettingsService.getLanguage();
    if (!language) {
      userSettingsService.setLanguage('no');
      language = 'no';
    }

    translate.setDefaultLang('no');
    translate.use(language);
    locale(language);
    this.initHeartbeat();
  }

  initHeartbeat() {
    this.idle.setIdle(AppConfig.settings.idleAfterSeconds);
    this.idle.setTimeout(AppConfig.settings.timeoutAfterSeconds);
    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    this.idle.onIdleEnd.pipe(takeUntil(this.onDestroy$)).subscribe(() => {
      this.isIdle = false;
      this.changeDetectionRef.detectChanges();
    });

    this.idle.onTimeout
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => this.doLogout());

    this.idle.onIdleStart
      .pipe(takeUntil(this.onDestroy$))
      .subscribe(() => (this.isIdle = true));

    this.idle.onTimeoutWarning.pipe(takeUntil(this.onDestroy$)).subscribe(
      countdown =>
        (this.countdownMessage$ = this.translate.get('app.idle-message', {
          countdown,
        })),
    );

    this.keepAlive.interval(AppConfig.settings.pingAfterSeconds);
    this.keepAlive.onPing.pipe(takeUntil(this.onDestroy$)).subscribe(() => {
      this.pingUrl = AppConfig.settings.pingUrl + '';
    });

    this.idle.watch();
  }

  doLogout() {
    if (AppConfig.settings.logoutUrl) {
      window.location.href = AppConfig.settings.logoutUrl;
    }
  }

  ngOnDestroy() {
    this.onDestroy$.next();
  }
}
