import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
} from '@angular/core';
import { SearchEventService } from '../../search-event.service';
import {
  SearchIndexType,
  SearchResult,
  SearchIndex,
} from 'src/apis/search/models';
import { Subscription } from 'rxjs';
import { PagingType } from 'src/app/core/models/enums/pagingType';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent implements OnInit, OnChanges, OnDestroy {
  @Output()
  search = new EventEmitter<string>();

  @Output()
  filter = new EventEmitter<SearchIndexType[]>();

  @Output()
  navigate: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  reset: EventEmitter<void> = new EventEmitter();

  @Input()
  result: SearchResult;

  @Input()
  indexes: SearchIndex[];

  @Input()
  initialQuery: any;

  @Input()
  pagingType: PagingType = PagingType.None;

  @Input()
  limit: number;

  @Input()
  offset: number;

  isLoading = false;
  subs: Subscription = new Subscription();
  error = '';

  constructor(private searchEventService: SearchEventService) {}

  ngOnInit() {
    this.subs.add(
      this.searchEventService.search.subscribe((query: string) => {
        if (!query || query.length === 0) {
          return;
        }

        this.searchEventService.resetState();
        this.startLoader();
        this.search.emit(query);
      }),
    );

    this.subs.add(
      this.searchEventService.filter.subscribe((indexes: SearchIndexType[]) => {
        if (!indexes || indexes.length === 0) {
          return;
        }

        this.searchEventService.resetState();
        this.startLoader();
        this.filter.emit(indexes);
      }),
    );

    this.subs.add(
      this.searchEventService.reset.subscribe(() => {
        this.searchEventService.resetState();
        this.reset.emit();
      }),
    );

    this.subs.add(
      this.searchEventService.navigate.subscribe((newOffset: number) => {
        this.navigate.emit(newOffset);
      }),
    );
  }

  ngOnChanges(changes: any) {
    if (changes.initialQuery && changes.initialQuery.currentValue) {
      // this.searchEventService.resetState();
      this.searchEventService.initialQuery = this.initialQuery.query;
    }

    if (changes.result && changes.result.currentValue) {
      const result = Object.assign({}, this.result);

      if (this.pagingType === PagingType.LoadMore) {
        result.items = [
          ...(this.searchEventService.result
            ? this.searchEventService.result.items
            : []),
          ...this.result.items,
        ]; // Just append the new items in case of load more
      }

      this.searchEventService.result = result;
      this.isLoading = false;
    }

    if (
      changes.indexes &&
      changes.indexes.currentValue &&
      changes.indexes.currentValue.length > 0
    ) {
      this.searchEventService.indexes = this.indexes;
    }

    if (changes.pagingType && changes.pagingType.currentValue) {
      this.searchEventService.pagingType = changes.pagingType.currentValue;
    }

    if (changes.limit && changes.limit.currentValue) {
      this.searchEventService.limit = changes.limit.currentValue;
    }

    if (changes.offset) {
      this.searchEventService.offset = changes.offset.currentValue;
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  private startLoader() {
    this.isLoading = true;

    setTimeout(() => {
      this.isLoading = false;
    }, 5000);
  }
}
