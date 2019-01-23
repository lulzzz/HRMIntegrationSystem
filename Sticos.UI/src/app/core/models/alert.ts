export class Alert {
  type: string;
  message: string;
}

export enum AlertType {
  success,
  error,
  info,
  warning,
}
