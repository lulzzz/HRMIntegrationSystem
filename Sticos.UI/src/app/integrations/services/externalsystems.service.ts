import { Injectable } from '@angular/core';
import {
  ExternalEconomySystem,
  ExternalSystem,
} from '../models/external-system';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UserCacheService } from 'src/app/core/services';
import { AppConfig } from '../../core/services/app-config-service';

@Injectable({
  providedIn: 'root',
})
export class ExternalSystemsService {
  externalSystems: ExternalEconomySystem[] = [];

  baseUrlTimereg = AppConfig.settings.apiUrls.timereg;
  baseUrlGoverenment = AppConfig.settings.apiUrls.altinn;
  customerId: string;

  constructor(
    private http: HttpClient,
    private userCacheService: UserCacheService,
  ) {
    this.userCacheService.ClaimsUser().subscribe(claimsUser => {
      this.customerId = claimsUser.customerId.toString();
    });
  }

  onGetExternalSystems(selectedCategory: number): Observable<ExternalSystem[]> {
    if (selectedCategory === 1) {
      return this.http
        .get<ExternalSystem[]>(
          `${this.baseUrlTimereg}/${this.customerId}/externalsystems`,
        )
        .pipe(map(_r => _r));
    }
    if (selectedCategory === 2) {
      return this.http
        .get<ExternalSystem[]>(
          `${this.baseUrlGoverenment}/${this.customerId}/externalsystems`,
        )
        .pipe(map(_r => _r));
    }
  }

  onGetExternalGovernmentSystems(): Observable<ExternalSystem[]> {
    return this.http
      .get<ExternalSystem[]>(
        `${this.baseUrlGoverenment}/${this.customerId}/externalsystems`,
      )
      .pipe(map(_r => _r));
  }

  getFeatures(externalSystemId): Observable<string> {
    return this.http
      .get<string>(
        `${this.baseUrlTimereg}/${
          this.customerId
        }/externalsystems/${externalSystemId}/features`,
      )
      .pipe(map(r => r));
  }
}
