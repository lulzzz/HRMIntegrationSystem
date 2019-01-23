import { Injectable } from '@angular/core';
import { IAppConfig } from '../models/app-config';
import { HttpClient } from '@angular/common/http';
import { ApiConfiguration as CommonApiConfiguration } from '@sticos/apis/common';
import { ApiConfiguration as TimeregApiConfiguration } from '@sticos/apis/timereg';
import { ApiConfiguration as AltinnApiConfiguration } from '@sticos/apis/altinn';
import { ApiConfiguration as IntegrationsApiConfiguration } from '@sticos/apis/integrations';
import { JL } from 'jsnlog';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AppConfig {
  static settings: IAppConfig;

  constructor(
    private http: HttpClient,
    private commonApiConfig: CommonApiConfiguration,
    private altinnApiConfig: AltinnApiConfiguration,
    private timeregApiConfig: TimeregApiConfiguration,
    private integrationsApiConfig: IntegrationsApiConfiguration,
  ) {}

  load() {
    let jsonFile = `assets/config/config.json`;
    if (!environment.production) {
      jsonFile = `assets/config/config.local.json`;
    }
    return new Promise<void>((resolve, reject) => {
      this.http
        .get(jsonFile)
        .toPromise()
        .then(response => {
          AppConfig.settings = <IAppConfig>response;
          this.setupApiUrls();
          this.setupJLOptions();
          resolve();
        })
        .catch((response: any) => {
          reject(
            `Could not load file '${jsonFile}': ${JSON.stringify(response)}`,
          );
        });
    });
  }

  setupApiUrls() {
    this.commonApiConfig.rootUrl = AppConfig.settings.apiUrls.common;
    this.timeregApiConfig.rootUrl = AppConfig.settings.apiUrls.timereg;
    this.altinnApiConfig.rootUrl = AppConfig.settings.apiUrls.altinn;
    this.integrationsApiConfig.rootUrl =
      AppConfig.settings.apiUrls.integrations;
  }

  setupJLOptions() {
    // Setting defaults for jsnlog
    JL.setOptions({
      defaultAjaxUrl: `${AppConfig.settings.apiUrls.common}/jsnlog.logger`,
    });
  }
}
