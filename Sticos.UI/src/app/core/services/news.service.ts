import { Injectable } from '@angular/core';
import { Observable, from } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { News } from 'src/app/core/models/news';
import { AppConfig } from '../../core/services/app-config-service';
import ODataStore from 'devextreme/data/odata/store';
import { UserCacheService } from './user-cache.service';
import { IClaimsUser } from 'src/apis/common/generated/models/iclaims-user';

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  private apiUrl = AppConfig.settings.apiUrls.news;

  constructor(private currentUserService: UserCacheService) {}

  get(id: number): Observable<News> {
    return this.currentUserService.ClaimsUser().pipe(
      switchMap((user: IClaimsUser) => {
        const store = new ODataStore({
          url: this.apiUrl + `/odata/${user.customerId}/news`,
          version: 4,
        });

        return from(store.byKey(id));
      }),
    );
  }

  getLatest(): Observable<News[]> {
    const date = new Date();
    const unitId = 1;
    const options = {
      filter: [
        [
          [
            [['fromDate', '<=', date], 'and', ['toDate', '>=', date]],
            'or',
            [['fromDate', '=', null], 'and', ['toDate', '=', null]],
            'or',
            [['fromDate', '=', null], 'and', ['toDate', '>=', date]],
            'or',
            [['fromDate', '<=', date], 'and', ['toDate', '=', null]],
          ],
          'and',
          [['unitId', '=', null], 'or', ['unitId', '=', unitId]],
        ],
      ],
      take: 30,
      sort: { selector: 'fromDate', desc: true },
    };

    return this.currentUserService.ClaimsUser().pipe(
      switchMap((user: IClaimsUser) => {
        const store = new ODataStore({
          url: this.apiUrl + `/odata/${user.customerId}/news`,
          version: 4,
        });

        return from(store.load(options));
      }),
    );
  }
}
