import { Injectable } from '@angular/core';
import { Alert, AlertType } from '../models/alert';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  alert: Alert;

  constructor() {}

  success(msg): Alert {
    this.alert = new Alert();
    this.alert.type = this.getType(AlertType.success);
    this.alert.message = msg;
    return this.alert;
  }

  error(msg): Alert {
    this.alert = new Alert();
    this.alert.type = this.getType(AlertType.error);
    this.alert.message = msg;
    return this.alert;
  }

  warning(msg): Alert {
    this.alert = new Alert();
    this.alert.type = this.getType(AlertType.warning);
    this.alert.message = msg;
    return this.alert;
  }

  danger(msg): Alert {
    this.alert = new Alert();
    this.alert.type = this.getType(AlertType.error);
    this.alert.message = msg;
    return this.alert;
  }

  getType(type: AlertType) {
    switch (type) {
      case AlertType.success:
        return 'success';
      case AlertType.error:
        return 'danger';
      case AlertType.warning:
        return 'warning';
    }
  }
}
