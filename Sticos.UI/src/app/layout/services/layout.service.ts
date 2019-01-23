import { Injectable, EventEmitter } from '@angular/core';

export class LayoutService {
  private oldOpen: boolean;
  private open = true;
  sidebarToggled: EventEmitter<boolean> = new EventEmitter();
  private lockToggle = false;

  sidebarToggle() {
    if (!this.lockToggle) {
      this.open = !this.open;
    }
    this.sidebarToggled.emit(this.open);
  }

  forceClose() {
    this.oldOpen = this.open;
    this.open = false;
    this.lockToggle = true;
    this.sidebarToggled.emit(this.isOpen);
  }

  removeForceClose() {
    this.lockToggle = false;
    this.open = this.oldOpen;
    this.sidebarToggled.emit(this.isOpen);
  }

  get isOpen() {
    return this.open;
  }
}
