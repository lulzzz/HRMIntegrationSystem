import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserSettingsService {
  getLanguage(): string {
    if (localStorage) {
      return localStorage['language'] || '';
    } else {
      return '';
    }
  }

  setLanguage(language: string) {
    if (localStorage) {
      localStorage['language'] = language;
    }
  }
}
