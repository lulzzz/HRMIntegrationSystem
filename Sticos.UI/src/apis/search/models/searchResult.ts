import { SearchResultItem } from './searchResultItem';
import { IndexHitCount } from './indexHitCount';
import { SearchStatus } from './searchStatus';

export interface SearchResult {
  items: Array<SearchResultItem>;
  hitCount: number;
  timeSpent: number;
  indexHitCount: Array<IndexHitCount>;
  status: SearchStatus;
}
