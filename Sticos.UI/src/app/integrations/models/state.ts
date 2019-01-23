export class StateInfo {
  state: State;
  stateType: StateType;
}

export enum State {
  Mapped = 1,
  Ignored = 2,
  Unsaved = 3,
  NoMap = 4,
  Suggestion = 5,
}

export enum StateType {
  Unknown = 0,
  A = 1,
  M = 2,
  I = 3,
  R = 4,
  S = 5,
}
