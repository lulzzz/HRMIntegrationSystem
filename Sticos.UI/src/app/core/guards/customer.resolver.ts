import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { UserCacheService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class CustomerResolver implements Resolve<any> {
  constructor(private userCacheService: UserCacheService) {}

  resolve(): Observable<any> {
    return this.userCacheService.ClaimsUser();
  }
}
