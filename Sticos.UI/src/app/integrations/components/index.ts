import { IntegrationRouterComponent } from '../pages/integration-router/integration-router.component';
export * from '../pages/integration-router/integration-router.component';
import { IntegrationMapperNotFoundComponent } from './integration-mapper-not-found/integration-mapper-not-found.component';
export * from './integration-mapper-not-found/integration-mapper-not-found.component';
import { TimeregAbsenceTypeMappingComponent } from './integration-timereg-wrapper/timereg-absence-type-mapping/timereg-absence-type-mapping.component';
export * from './integration-timereg-wrapper/timereg-absence-type-mapping/timereg-absence-type-mapping.component';
import { BadgeComponentComponent } from './badge-component/badge-component.component';
import { AltinnEmployeeMappingComponent } from './integration-altinn-wrapper/altinn-employee-mapping/altinn-employee-mapping.component';

export * from './badge-component/badge-component.component';
import { TimeregEmployeeMappingComponent } from './integration-timereg-wrapper/timereg-employee-mapping/timereg-employee-mapping.component';
export * from './integration-timereg-wrapper/timereg-employee-mapping/timereg-employee-mapping.component';
import { TimeregUnitMappingComponent } from './integration-timereg-wrapper/timereg-unit-mapping/timereg-unit-mapping.component';
export * from './integration-timereg-wrapper/timereg-unit-mapping/timereg-unit-mapping.component';
import { AltinnUnitMappingComponent } from './integration-altinn-wrapper/altinn-unit-mapping/altinn-unit-mapping.component';
export * from './integration-altinn-wrapper/altinn-unit-mapping/altinn-unit-mapping.component';

export const components = [
  IntegrationRouterComponent,
  IntegrationMapperNotFoundComponent,
  TimeregAbsenceTypeMappingComponent,
  BadgeComponentComponent,
  TimeregEmployeeMappingComponent,
  TimeregUnitMappingComponent,
  AltinnUnitMappingComponent,
  AltinnEmployeeMappingComponent,
];
