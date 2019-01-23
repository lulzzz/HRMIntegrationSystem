import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserCacheService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class CustomerAdminGuard implements CanActivate {
  constructor(
    private userCacheService: UserCacheService,
    private router: Router,
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean | Observable<boolean> | Promise<boolean> {
    return this.userCacheService.Current().pipe(
      map(user => {
        if (!user.isPersonalCustomerAdmin) {
          this.router.navigateByUrl('/no-access');
        }
        return user.isPersonalCustomerAdmin;
      }),
    );
  }
}
