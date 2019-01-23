import {
  EventEmitter,
  Output,
  Input,
  Component,
  OnInit,
  ViewChild,
  OnChanges,
} from '@angular/core';

import { SearchPage } from '../../../models/searchPage';

@Component({
  selector: 'app-search-pager',
  templateUrl: './search-pager.component.html',
  styleUrls: ['./search-pager.component.scss'],
})
export class SearchPagerComponent implements OnInit, OnChanges {
  pageIndex = 0;
  pages: Array<SearchPage> = [];
  maxSize = 6;

  @Input()
  pageSize: number;
  @Input()
  total: number;
  @Input()
  currentPage: number;
  @Output()
  pageChange: EventEmitter<number> = new EventEmitter<number>();

  constructor() {}

  ngOnInit() {
    // this.updateLinks();
  }

  ngOnChanges(changes) {
    if (changes.currentPage && changes.currentPage.currentValue) {
      this.updateLinks();
    }
  }

  nextPage() {
    this.gotoPage(this.currentPage + 1);
  }

  prevPage() {
    this.gotoPage(this.currentPage - 1);
  }

  gotoPage(newPage: number) {
    this.currentPage = newPage;
    this.pageChange.emit(newPage);
  }

  updateLinks(): void {
    const correctedCurrentPage = this.outOfBoundCorrection();

    if (correctedCurrentPage !== this.currentPage) {
      setTimeout(() => {
        this.pageChange.emit(correctedCurrentPage);
        this.pages = this.createPageArray(
          this.currentPage,
          this.pageSize,
          this.total,
          this.maxSize,
        );
      });
    } else {
      this.pages = this.createPageArray(
        this.currentPage,
        this.pageSize,
        this.total,
        this.maxSize,
      );
    }
  }

  private outOfBoundCorrection(): number {
    const totalPages = Math.ceil(this.total / this.pageSize);
    if (totalPages < this.currentPage && 0 < totalPages) {
      return totalPages;
    } else if (this.currentPage < 1) {
      return 1;
    }

    return this.currentPage;
  }

  private createPageArray(
    currentPage: number,
    itemsPerPage: number,
    totalItems: number,
    paginationRange: number,
  ): SearchPage[] {
    // paginationRange could be a string if passed from attribute, so cast to number.
    paginationRange = +paginationRange;
    const pages: SearchPage[] = [];
    const totalPages = Math.ceil(totalItems / itemsPerPage);
    const halfWay = Math.ceil(paginationRange / 2);

    const isStart = currentPage <= halfWay;
    const isEnd = totalPages - halfWay < currentPage;
    const isMiddle = !isStart && !isEnd;

    const ellipsesNeeded = paginationRange < totalPages;
    let i = 1;

    while (i <= totalPages && i <= paginationRange) {
      let label;
      const pageNumber = this.calculatePageNumber(
        i,
        currentPage,
        paginationRange,
        totalPages,
      );
      const openingEllipsesNeeded = i === 2 && (isMiddle || isEnd);
      const closingEllipsesNeeded =
        i === paginationRange - 1 && (isMiddle || isStart);
      if (ellipsesNeeded && (openingEllipsesNeeded || closingEllipsesNeeded)) {
        label = '...';
      } else {
        label = pageNumber;
      }
      pages.push({
        label: label,
        number: pageNumber,
        visible: true,
      });
      i++;
    }
    return pages;
  }

  /**
   * Given the position in the sequence of pagination links [i],
   * figure out what page number corresponds to that position.
   */
  private calculatePageNumber(
    i: number,
    currentPage: number,
    paginationRange: number,
    totalPages: number,
  ) {
    const halfWay = Math.ceil(paginationRange / 2);
    if (i === paginationRange) {
      return totalPages;
    } else if (i === 1) {
      return i;
    } else if (paginationRange < totalPages) {
      if (totalPages - halfWay < currentPage) {
        return totalPages - paginationRange + i;
      } else if (halfWay < currentPage) {
        return currentPage - halfWay + i;
      } else {
        return i;
      }
    } else {
      return i;
    }
  }
}
