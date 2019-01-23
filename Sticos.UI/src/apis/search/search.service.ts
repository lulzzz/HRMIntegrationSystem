import { Injectable } from '@angular/core';
import { AppConfig } from '../../app/core/services/app-config-service';
import { Observable, of } from 'rxjs';
import { SearchQuery, SearchResult } from './models';
import { HttpClient } from '@angular/common/http';
import { delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private apiUrl = AppConfig.settings.apiUrls.personal + '/api/search';

  constructor(private httpClient: HttpClient) {}

  search(search: SearchQuery): Observable<SearchResult> {
    if (
      !search ||
      !search.indexTypes ||
      search.indexTypes.length === 0 ||
      search.type === '' ||
      search.limit === 0
    ) {
      return of(null);
    }

    return this.httpClient.post<SearchResult>(this.apiUrl, search);
  }
}
