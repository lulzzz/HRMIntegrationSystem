import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';

import { TitleChange } from '../models/title-change';

@Injectable({
  providedIn: 'root',
})
export class DashboardGridService {
  private _title = new ReplaySubject<TitleChange>();

  titleCahnge(change: TitleChange) {
    this._title.next(change);
  }

  onTitleChange(): Observable<TitleChange> {
    return this._title.asObservable();
  }
}
