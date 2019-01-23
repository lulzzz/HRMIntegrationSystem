import 'reflect-metadata/reflect';
import { IntegrationCategoryEnum } from '../models/integration-category';

export interface IIntegrationMapperData {
  category: IntegrationCategoryEnum;
}

export function SticosIntegrationMapper(config: IIntegrationMapperData) {
  return function(target) {
    Reflect.defineMetadata('__sticosIntegrationMapper', config, target);

    Object.defineProperty(target, 'sticosIntegrationMapper', {
      configurable: false,
      get: () => Reflect.getMetadata('__sticosIntegrationMapper', target),
    });
  };
}
