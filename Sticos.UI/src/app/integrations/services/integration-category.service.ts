import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { IntegrationCategory } from '../models/integration-category';
import { UserCacheService } from 'src/app/core/services';
import { AppConfig } from '../../core/services/app-config-service';

@Injectable({
  providedIn: 'root',
})
export class IntegrationCategoryService {
  baseUrl = AppConfig.settings.apiUrls.integrations;
  customerId: any;

  constructor(
    private http: HttpClient,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
  }

  getCategories(): Observable<IntegrationCategory[]> {
    return this.http
      .get<IntegrationCategory[]>(
        `${this.baseUrl}/${this.customerId}/integrations/categories`,
      )
      .pipe(map(_r => _r));
  }
}
