import { SearchIndexType } from './searchIndexType';

export interface SearchQuery {
  query: string;
  offset: number;
  limit: number;
  indexTypes: SearchIndexType[];
  type?: string;
}
