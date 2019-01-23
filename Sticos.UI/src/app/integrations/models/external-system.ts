import { ICode } from './ICode';

export class ExternalSystem implements ICode {
  id: string;
  type: string;
  value: string;
  order: string;
}

export enum ExternalEconomySystem {
  Unknown = 0,
  // Xledger = 1,
  UniMicro = 2,
  // Unit4 = 3,
}
