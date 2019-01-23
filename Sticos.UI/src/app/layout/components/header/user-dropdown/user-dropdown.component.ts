import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { UserCacheService, UserSettingsService } from 'src/app/core/services';
import { locale } from 'devextreme/localization';
import { Router } from '@angular/router';
import { AppConfig } from '../../../../core/services/app-config-service';
import { IUser } from '@sticos/apis/common';

@Component({
  selector: 'app-user-dropdown',
  templateUrl: './user-dropdown.component.html',
  styleUrls: ['./user-dropdown.component.scss'],
})
export class UserDropdownComponent implements OnInit {
  currentLanguage: any;
  user: IUser = {} as IUser;
  selectedLanguage: any;
  settingsPopupVisible: boolean;

  languages = [
    { name: 'English', value: 'en' },
    { name: 'Norsk', value: 'no' },
  ];

  constructor(
    private userCacheService: UserCacheService,
    private translateService: TranslateService,
    private userSettingsService: UserSettingsService,
    private router: Router,
  ) {
    const lang = this.userSettingsService.getLanguage();
    this.selectedLanguage = this.languages.find(x => x.value === lang);
    this.currentLanguage = this.selectedLanguage;
  }

  ngOnInit() {
    this.settingsPopupVisible = false;
    this.userCacheService.Current().subscribe(data => {
      this.user = data;
    });
  }

  languageChanged(lang) {
    this.currentLanguage = lang;
  }

  saveSettings() {
    this.selectedLanguage = this.currentLanguage;
    this.userSettingsService.setLanguage(this.currentLanguage.value);
    this.translateService.use(this.currentLanguage.value);
    locale(this.selectedLanguage.value);
    this.settingsPopupVisible = false;
    parent.document.location.reload();
  }

  cancel() {
    this.currentLanguage = this.selectedLanguage;
    this.settingsPopupVisible = false;
  }

  logout() {
    const logoutUrl = AppConfig.settings.logoutUrl;
    this.router.navigateByUrl(logoutUrl);
  }
}
