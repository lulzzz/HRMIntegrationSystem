import { Injectable } from '@angular/core';
import { CacheService } from 'ng2-cache';
import {
  UserService,
  IUser,
  CurrentUserService,
  IClaimsUser,
} from '@sticos/apis/common';
import { map, share, flatMap, switchMap } from 'rxjs/operators';
import { of, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserCacheService {
  private getCurrentUserObservable: Observable<IUser>;
  private getClaimsUserObservable: Observable<IClaimsUser>;

  constructor(
    private userService: UserService,
    private cacheService: CacheService,
    private currentUserService: CurrentUserService,
  ) {}

  Current(): Observable<IUser> {
    const key = 'currentuser';
    return of(this.cacheService.get(key)).pipe(
      switchMap((cachedUser: IUser) => {
        if (cachedUser) {
          return of(cachedUser);
        }
        if (!this.getCurrentUserObservable) {
          this.getCurrentUserObservable = this.ClaimsUser().pipe(
            flatMap((claimsUser: IClaimsUser) => {
              return this.userService
                .GetUser({
                  userId: claimsUser.userId,
                  customerId: claimsUser.customerId.toString(),
                })
                .pipe(
                  map((user: IUser) => {
                    this.cacheService.set(key, user, { maxAge: 5 * 60 });
                    return user;
                  }),
                );
            }),
            share(),
          );
        }
        return this.getCurrentUserObservable;
      }),
    );
  }

  ClaimsUser(): Observable<IClaimsUser> {
    const key = 'claimsuser';
    return of(this.cacheService.get(key)).pipe(
      switchMap((claimsUser: IClaimsUser) => {
        if (claimsUser) {
          return of(claimsUser);
        }
        if (!this.getClaimsUserObservable) {
          this.getClaimsUserObservable = this.currentUserService.Get().pipe(
            map((data: IClaimsUser) => {
              if (!claimsUser) {
                this.cacheService.set(key, data, { maxAge: 5 * 60 });
              }
              return data;
            }),
            share(),
          );
        }
        return this.getClaimsUserObservable;
      }),
      share(),
    );
  }
}
