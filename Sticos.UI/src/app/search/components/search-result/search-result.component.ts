import { Component, OnInit, OnDestroy } from '@angular/core';
import { SearchResultItem, SearchResult } from 'src/apis/search/models';
import { SearchEventService } from '../../search-event.service';
import { Subscription } from 'rxjs';
import { PagingType } from 'src/app/core/models/enums/pagingType';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.scss'],
})
export class SearchResultComponent implements OnInit, OnDestroy {
  total = 0;
  items: SearchResultItem[] = [];
  stats = '';
  limit: number;
  subs = new Subscription();
  showPager = false;
  showLoadMore = false;
  pagingType: PagingType;
  isLoading: boolean;

  constructor(private searchEventService: SearchEventService) {}

  ngOnInit() {
    const me = this;
    this.subs.add(
      me.searchEventService.result$.subscribe((result: SearchResult) => {
        if (!result) {
          return;
        }

        me.isLoading = false;

        me.pagingType = me.searchEventService.pagingType;
        me.limit = me.searchEventService.limit;

        me.total = result.hitCount || 0;
        me.items = result.items || [];

        me.stats = me.getStats();

        if (me.pagingType === PagingType.Paging) {
          me.showPager = true;
        } else if (me.pagingType === PagingType.LoadMore) {
          me.showLoadMore = me.items.length < me.total;
        }
      }),
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  getCurrentPage(): number {
    const limit = this.searchEventService.limit || 0;
    const offset = this.searchEventService.offset || 0;

    if (limit === 0) {
      return 1;
    }

    if (offset === 0) {
      return 1;
    }

    return Math.ceil(offset / limit) + 1;
  }

  getStats(): string {
    let pagingInfo = '';
    let statsInfo = 'Ingen treff';

    const pageTotal = this.items.length;
    const timeSpent = this.searchEventService.result.timeSpent;

    let itemStart = 0;
    let itemEnd = pageTotal;
    if (this.searchEventService.pagingType === PagingType.Paging) {
      itemStart = this.searchEventService.offset;
      itemEnd = itemStart + this.searchEventService.limit;

      if (this.searchEventService.limit > pageTotal) {
        itemEnd = itemStart + pageTotal;
      }
    }

    if (this.total > 0 && pageTotal > 0) {
      pagingInfo = `Viser ${itemStart + 1} til ${itemEnd} av`;
      statsInfo = `${this.total} treff`;
    }

    statsInfo += ` (${(timeSpent / 1000).toFixed(2)}s)`;

    return [pagingInfo, statsInfo].join(' ');
  }

  pageChange(newPage: number) {
    const pageIndex = newPage - 1;
    const newOffset = pageIndex * this.searchEventService.limit;

    this.items = [];
    this.isLoading = true;

    this.searchEventService.navigate.emit(newOffset);
  }

  loadMore() {
    const newOffset =
      this.searchEventService.offset + this.searchEventService.limit;

    this.isLoading = true;

    this.searchEventService.navigate.emit(newOffset);
  }
}
