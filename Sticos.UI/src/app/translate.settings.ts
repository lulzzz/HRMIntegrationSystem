import {
  TranslateModule,
  TranslateLoader,
  MissingTranslationHandler,
  TranslateModuleConfig,
} from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { AppMissingTranslationHandler } from './core/missing-translation.handler';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
const translateSettings: TranslateModuleConfig = {
  missingTranslationHandler: {
    provide: MissingTranslationHandler,
    useClass: AppMissingTranslationHandler,
  },
  useDefaultLang: false,
  loader: {
    provide: TranslateLoader,
    useFactory: createTranslateLoader,
    deps: [HttpClient],
  },
  isolate: false,
};

export { translateSettings };
