import { SearchIndex } from './searchIndex';

export interface SearchResultItem {
  index: SearchIndex;
  title: string;
  summary: string;
  link?: string;
  linkTarget?: string;
  score: number;
  fields: { [key: string]: string }[];
  additionalLinkHref?: string;
  additionalLinkTarget?: string;
  additionalLinkTitle?: string;
}
