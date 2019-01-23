export class EntityMap {
  integrationId: number;

  localId: number;

  externalId: number;

  isIgnored: boolean;

  entityType: number;

  mapPropertyKey: string;
}

export enum ExternalDataEnum {
  Employee = 1,
  Unit = 2,
  AbsenceCode = 3,
}
