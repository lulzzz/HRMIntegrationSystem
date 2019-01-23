import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  globalSearch: EventEmitter<string> = new EventEmitter<string>();
}
