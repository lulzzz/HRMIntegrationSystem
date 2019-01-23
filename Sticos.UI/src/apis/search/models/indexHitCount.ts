import { SearchIndexType } from './searchIndexType';

export interface IndexHitCount {
  searchIndexType: SearchIndexType;
  hitCount: number;
}
