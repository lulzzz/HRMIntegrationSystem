export interface IAppConfig {
  idleAfterSeconds: number;
  timeoutAfterSeconds: number;
  pingAfterSeconds: number;
  pingUrl: string;
  logoutUrl: string;
  apiUrls: {
    common: string;
    timereg: string;
    altinn: string;
    integrations: string;
    news: string;
    filePreview: string;
    personal: string;
  };
  personalUnitListUrl: string;
}
