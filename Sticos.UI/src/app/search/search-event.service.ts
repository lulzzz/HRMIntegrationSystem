import { Injectable, EventEmitter } from '@angular/core';
import {
  SearchIndexType,
  SearchResult,
  SearchIndex,
} from 'src/apis/search/models';
import { BehaviorSubject, Observable } from 'rxjs';
import { PagingType } from '../core/models/enums/pagingType';
import { takeWhile } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SearchEventService {
  get initialQuery(): string {
    return this._initialQuery$.getValue();
  }
  set initialQuery(value: string) {
    this._initialQuery$.next(value);
  }
  get result(): SearchResult {
    return this._result$.getValue();
  }
  set result(value: SearchResult) {
    this._result$.next(value);
  }
  get indexes(): SearchIndex[] {
    return this._indexes$.getValue();
  }
  set indexes(value: SearchIndex[]) {
    this._indexes$.next(value);
  }
  get limit(): number {
    return this._limit$.getValue();
  }
  set limit(value: number) {
    this._limit$.next(value);
  }
  get offset(): number {
    return this._offset$.getValue();
  }
  set offset(value: number) {
    this._offset$.next(value);
  }

  get initialQuery$(): Observable<string> {
    return this._initialQuery$.asObservable();
  }
  get result$(): Observable<SearchResult> {
    return this._result$.asObservable();
  }
  get indexes$(): Observable<SearchIndex[]> {
    return this._indexes$.asObservable();
  }

  get limit$(): Observable<number> {
    return this._limit$.asObservable();
  }
  get offset$(): Observable<number> {
    return this._offset$.asObservable();
  }

  filter = new EventEmitter<SearchIndexType[]>();
  search = new EventEmitter<string>();
  reset = new EventEmitter();
  navigate = new EventEmitter<number>();
  pagingType: PagingType;

  private _initialQuery$ = new BehaviorSubject<string>(null);
  private _result$: BehaviorSubject<SearchResult> = new BehaviorSubject(null);
  private _indexes$: BehaviorSubject<SearchIndex[]> = new BehaviorSubject(null);
  private _limit$: BehaviorSubject<number> = new BehaviorSubject(null);
  private _offset$: BehaviorSubject<number> = new BehaviorSubject(null);

  constructor() {}

  resetState() {
    this._result$.next(null);
    this._offset$.next(null);
    this._indexes$.next(null);
    this._initialQuery$.next(null);
  }
}
