import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { UserCacheService } from 'src/app/core/services';

@Injectable({
  providedIn: 'root',
})
export class FeatureGuard implements CanActivate {
  constructor(
    private userCacheService: UserCacheService,
    private router: Router,
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean> {
    return this.userCacheService.Current().pipe(
      switchMap(user => {
        // some code to check features
        return of(true);
      }),
    );
  }
}
