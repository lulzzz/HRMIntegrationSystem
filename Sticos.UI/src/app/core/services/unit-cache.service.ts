import { Injectable } from '@angular/core';
import { CacheService } from 'ng2-cache';
import { UnitService, Unit } from '@sticos/apis/common';
import { Observable, of } from 'rxjs';
import { Md5 } from 'ts-md5/dist/md5';
import { map, switchMap } from 'rxjs/operators';
import { share } from 'rxjs/internal/operators/share';

@Injectable({
  providedIn: 'root',
})
export class UnitCacheService {
  private getUnitsObservable: Observable<Unit[]>;

  constructor(
    private unitService: UnitService,
    private cacheService: CacheService,
  ) {}

  GetUnits(params: UnitService.GetUnitsParams) {
    const key = `getUnits-${Md5.hashStr(JSON.stringify(params))}`;

    return of(this.cacheService.get(key)).pipe(
      switchMap(cachedUnits => {
        if (cachedUnits) {
          return of(cachedUnits);
        }
        if (!this.getUnitsObservable) {
          this.getUnitsObservable = this.unitService.GetUnits(params).pipe(
            map((data: Unit[]) => {
              this.cacheService.set(key, data, { maxAge: 5 * 60 });

              return data;
            }),
            share(),
          );
        }
        return this.getUnitsObservable;
      }),
    );
  }
}
