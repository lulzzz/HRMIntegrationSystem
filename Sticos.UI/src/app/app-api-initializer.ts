import { ApiConfiguration as CommonApiConfiguration } from '@sticos/apis/common';
import { ApiConfiguration as TimeregApiConfiguration } from '@sticos/apis/timereg';
import { ApiConfiguration as AltinnApiConfiguration } from '@sticos/apis/altinn';
import { ApiConfiguration as IntegrationsApiConfiguration } from '@sticos/apis/integrations';
import { Provider, APP_INITIALIZER } from '@angular/core';
import { AppConfig } from './core/services/app-config-service';

export function initApiConfiguration(appConfig: AppConfig): Function {
  return () => {
    return appConfig.load();
  };
}

export const INIT_API_CONFIGURATION: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initApiConfiguration,
  deps: [AppConfig],
  multi: true,
};
