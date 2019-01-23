import { AbsenseExportComponent } from './absense-export/absense-export.component';
export * from './absense-export/absense-export.component';
import { IntegrationAddComponent } from './integration-add/integration-add.component';
export * from './integration-add/integration-add.component';
import { IntegrationsListComponent } from './integrations-list/integrations-list.component';
export * from './integrations-list/integrations-list.component';
import { IntegrationAltinnWrapperComponent } from '../components/integration-altinn-wrapper/integration-altinn-wrapper.component';
export * from '../components/integration-altinn-wrapper/integration-altinn-wrapper.component';
import { IntegrationTimeregWrapperComponent } from '../components/integration-timereg-wrapper/integration-timereg-wrapper.component';
export * from '../components/integration-timereg-wrapper/integration-timereg-wrapper.component';

export const pages = [
  AbsenseExportComponent,
  IntegrationAddComponent,
  IntegrationsListComponent,
  IntegrationAltinnWrapperComponent,
  IntegrationTimeregWrapperComponent,
];
