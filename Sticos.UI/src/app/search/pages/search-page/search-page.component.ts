import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { Observable, of, Subscription, Subject } from 'rxjs';
import {
  SearchResult,
  SearchQuery,
  SearchIndex,
  SearchResultItem,
  SearchIndexType,
} from 'src/apis/search/models';
import { SearchService } from 'src/apis/search/search.service';
import { SearchIndexService } from 'src/apis/search/searchindex.service';
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { DomSanitizer } from '@angular/platform-browser';
import { EventService } from 'src/app/core/services/event.service';
import { PagingType } from 'src/app/core/models/enums/pagingType';

@Component({
  selector: 'app-search-page',
  templateUrl: './search-page.component.html',
  styleUrls: ['./search-page.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class SearchPageComponent implements OnInit, OnDestroy {
  limit = 10;
  offset = 0;
  indexes: SearchIndex[] = [];
  result$: Observable<SearchResult> = of(null);
  indexes$: Observable<SearchIndex[]> = of([]);
  currentSearch: SearchQuery = {
    indexTypes: [],
    limit: this.limit,
    offset: this.offset,
    query: '',
  };
  query: any;
  pagingType = PagingType.LoadMore;
  subs: Subscription = new Subscription();
  searchSubject$ = new Subject();

  private isMobile = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private searchService: SearchService,
    private searchIndexService: SearchIndexService,
    private domSanitizer: DomSanitizer,
    private eventService: EventService,
  ) {
    this.isMobile = this.route.snapshot.queryParams['isMobile'] === 'True';

    if (this.route.snapshot.queryParams['query']) {
      this.query = { query: this.route.snapshot.queryParams['query'] };
    }

    const me = this;
    me.result$ = me.searchSubject$.pipe(
      switchMap(() => {
        if (this.indexes && this.indexes.length > 0) {
          return of(this.indexes);
        }
        return me.searchIndexService.getAll();
      }),
      switchMap((indexes: SearchIndex[]) => {
        if (!indexes) {
          return of(null);
        }

        if (!me.indexes || me.indexes.length === 0) {
          me.indexes = indexes;
        }

        switch (me.currentSearch.type) {
          case 'normal':
            me.currentSearch.indexTypes = me.indexes.map(item => item.type);
            break;

          case 'navigate': // keep indexes for navigate and filter
          case 'filter':
            break;
        }

        return me.searchService.search(me.currentSearch);
      }),
      map((result: SearchResult) => {
        if (!result) {
          return null;
        }

        return me.mapSearchResult(result, me.currentSearch.query);
      }),
    );
  }

  ngOnInit() {
    this.subs.add(
      this.eventService.globalSearch.subscribe((query: string) => {
        this.query = { query: query };
        this.router.navigateByUrl('/search?query=' + this.query.query);
      }),
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  navigate(newOffset: number) {
    this.offset = newOffset;
    this.currentSearch.type = 'navigate';
    this.currentSearch.offset = this.offset;

    this.runSearch('navigate');
  }

  search(query: string) {
    this.offset = 0;
    this.currentSearch.type = 'normal';
    this.currentSearch.query = query;
    this.currentSearch.offset = this.offset;

    this.runSearch('search');
  }

  filter(indexTypes: SearchIndexType[]) {
    this.offset = 0;
    this.currentSearch.type = 'filter';
    this.currentSearch.offset = this.offset;
    this.currentSearch.indexTypes = indexTypes;

    this.runSearch('filter');
  }

  reset() {
    this.currentSearch = null;
    this.result$ = of(null);
  }

  private runSearch(type: string) {
    this.searchSubject$.next({ type: type });
  }

  private mapSearchResult(input: SearchResult, query: string): SearchResult {
    input.items.forEach((value: SearchResultItem) => {
      const link = this.getLink(value, this.isMobile, query);
      value.link = link.href;
      value.linkTarget = link.target;

      const stripped = this.stripHtml(value.summary);
      const highlighted = this.highlightWords(stripped, query);
      value.summary = highlighted;

      if (
        value.index &&
        value.index.type === SearchIndexType.Paragrafer &&
        link.additionalLink
      ) {
        value['additionalLink'] = link.additionalLink;
      }
    });

    return input;
  }

  private stripHtml(html: string): string {
    if (!html || html === '') {
      return '';
    }

    const strippedHtml = html.replace(/(<([^>]+)>)/gi, '');

    return strippedHtml;
  }

  private highlightWords(html: string, query: string): string {
    const highlighTarget = query;
    const regEx = new RegExp(/^[a-zæøåA-ZÆØÅ§\ \.\_\-\d\.]{3,}$/);
    const isSearchTermValid = regEx.test(highlighTarget);
    if (!isSearchTermValid) {
      return html;
    }

    if (highlighTarget && highlighTarget !== '') {
      const regExp = new RegExp(
        `(([A-ZÆØÅa-zæøå0-9])+${highlighTarget}([A-ZÆØÅa-zæøå0-9])+|([A-ZÆØÅa-zæøå0-9])+${highlighTarget}|${highlighTarget}[A-ZÆØÅa-zæøå0-9]+|${highlighTarget})`,
        'i',
      );

      const results = regExp.exec(html);

      if (results && results.length > 0) {
        const before = html.substr(0, results.index);
        const after = html.substr(results.index + results[0].length);

        const indexOpenTag = before.lastIndexOf('<');
        const indexCloseTag = before.lastIndexOf('>');
        const indexOpenTagAfter = after.indexOf('<');
        const indexCloseTagAfter = after.indexOf('>');

        if (
          indexOpenTag <= indexCloseTag &&
          indexOpenTagAfter <= indexCloseTagAfter
        ) {
          return `${before}<span style="font-style: italic;text-decoration: underline;color: green;">${
            results[0]
          }</span>${this.highlightWords(after, query)}`;
        } else {
          return `${before}${results[0]}${this.highlightWords(after, query)}`;
        }
      }
    }

    return html;
  }

  private getLink(
    item: SearchResultItem,
    isMobile: boolean,
    query: string,
  ): any {
    const returnLink = { target: '' };
    if (!item) {
      return returnLink;
    }

    let href = '';
    let additionalLinkHref;
    let additionalLinkTitle;
    let additionalLinkTarget;
    let target = '_self';
    if (
      item.index &&
      item.index.type &&
      item.index.type === SearchIndexType.Google
    ) {
      href = item.fields[1].value;
      target = '_blank';
    } else if (
      item.index &&
      item.index.type &&
      item.index.type === SearchIndexType.Paragrafer
    ) {
      href =
        '/felles/sok/dispatch?query=' +
        query +
        '&sticosId=' +
        this.getFieldValue(item, 'sticosid') +
        '&linkBokmerke=' +
        this.getFieldValue(item, 'linkbokmerke') +
        '&refId=' +
        this.getFieldValue(item, 'refid') +
        '&indexType=' +
        SearchIndexType.Lover;
      additionalLinkHref =
        '/felles/sok/dispatch?query=' +
        query +
        '&sticosId=' +
        this.getFieldValue(item, 'sticosid') +
        '&linkBokmerke=&refId=' +
        this.getFieldValue(item, 'ref') +
        '&indexType=' +
        item.index.type;
      additionalLinkTarget = target;

      additionalLinkTitle = item.title.split('§')[0];
    } else {
      href =
        '/felles/sok/dispatch?query=' +
        query +
        '&sticosId=' +
        this.getFieldValue(item, 'sticosid') +
        '&linkBokmerke=' +
        this.getFieldValue(item, 'linkbokmerke') +
        '&refId=' +
        this.getFieldValue(item, 'refid') +
        '&indexType=' +
        item.index.type;
    }

    if (isMobile === true) {
      href += '&isMobile=True';
    }

    returnLink['href'] = this.domSanitizer.bypassSecurityTrustUrl(href);
    returnLink['target'] = target;

    if (additionalLinkHref && additionalLinkTitle) {
      returnLink['additionalLink'] = {
        href: additionalLinkHref,
        target: additionalLinkTarget,
        title: additionalLinkTitle,
      };
    }

    return returnLink;
  }

  private getFieldValue(item: SearchResultItem, key: string): string {
    const prop = item.fields.filter(field => field.key === key);
    if (prop.length === 1) {
      return prop[0].value;
    }

    return '';
  }
}
