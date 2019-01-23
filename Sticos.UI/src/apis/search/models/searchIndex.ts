import { SearchIndexType } from './searchIndexType';

export interface SearchIndex {
  providerType: number;
  contentType: number;
  isCommon: boolean;
  criteria: any;
  type: SearchIndexType;
  name: string;
  indexName: string;
  selected?: boolean;
  count?: number;
  order: number;
  hidden: boolean;
}
