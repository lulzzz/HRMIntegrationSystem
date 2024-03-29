import { ErrorHandler } from '@angular/core';
import { JL } from 'jsnlog';

export class UncaughtExceptionHandler extends ErrorHandler {
  constructor() {
    super();
  }
  handleError(error: any) {
    JL().fatalException('Uncaught Exception', error);
    super.handleError(error);
  }
}
