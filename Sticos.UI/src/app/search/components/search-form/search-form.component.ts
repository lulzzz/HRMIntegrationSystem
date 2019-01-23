import { Component, OnInit, OnChanges, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SearchIndex, SearchResult } from 'src/apis/search/models';
import { Subscription } from 'rxjs';
import { SearchEventService } from '../../search-event.service';

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss'],
})
export class SearchFormComponent implements OnInit, OnChanges, OnDestroy {
  indexes: SearchIndex[];
  result: SearchResult;
  form: FormGroup;
  currentIndex = -1;
  subs: Subscription = new Subscription();

  constructor(
    private _fb: FormBuilder,
    private searchEventService: SearchEventService,
  ) {
    this.form = this._fb.group({
      query: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.pattern(/^[a-zæøåA-ZÆØÅ§\ \.\_\-\d\.]{3,}$/),
        ],
      ],
    });
  }

  ngOnInit() {
    this.subs.add(
      this.searchEventService.indexes$.subscribe((indexes: SearchIndex[]) => {
        if (indexes && indexes.length > 0) {
          this.indexes = indexes;
          this.initIndexes();
        }
      }),
    );

    this.subs.add(
      this.searchEventService.result$.subscribe((result: SearchResult) => {
        if (result) {
          this.result = result;
          if (this.result.indexHitCount) {
            this.updateIndexCount();
          }
        }
      }),
    );

    this.subs.add(
      this.searchEventService.initialQuery$.subscribe((query: string) => {
        query = query || '';
        if (query !== '') {
          this.form.patchValue({ query: query });

          this.doSearch();
        }
      }),
    );
  }

  ngOnChanges(changes) {}

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  initIndexes() {
    this.indexes = this.indexes.sort(
      (left: SearchIndex, right: SearchIndex): number => {
        return left.order < right.order ? -1 : left.order > right.order ? 1 : 0;
      },
    );
  }

  updateIndexCount() {
    this.indexes.forEach((value: SearchIndex, index: number) => {
      const matchingIndexes = this.result.indexHitCount.filter(
        ih => ih.searchIndexType === value.type,
      );
      if (matchingIndexes.length === 1) {
        value.count = matchingIndexes[0].hitCount;
      }
    });
  }

  doSearch() {
    this.currentIndex = -1;
    this.result = null;
    const formValues = this.form.value;
    const query = formValues.query;

    this.searchEventService.search.emit(query);
  }

  doFilter() {
    let indexTypes = [];
    if (this.currentIndex === -1) {
      indexTypes = this.indexes.map((item: SearchIndex) => item.type);
    } else {
      indexTypes = [this.currentIndex];
    }

    this.searchEventService.filter.emit(indexTypes);
  }

  clearSearch() {
    this.result = null;
    this.indexes = [];
    this.currentIndex = -1;
    this.searchEventService.reset.emit();
  }

  setCurrentIndex(indexType: number) {
    this.currentIndex = indexType;

    this.doFilter();
  }
}
