import { Injectable } from '@angular/core';
import { AppConfig } from '../../app/core/services/app-config-service';
import { Observable, of } from 'rxjs';
import { SearchIndex } from './models';
import { HttpClient } from '@angular/common/http';
import { delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SearchIndexService {
  private apiUrl = AppConfig.settings.apiUrls.personal + '/api/searchindex';

  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<SearchIndex[]> {
    return this.httpClient.get<SearchIndex[]>(this.apiUrl);
  }
}
