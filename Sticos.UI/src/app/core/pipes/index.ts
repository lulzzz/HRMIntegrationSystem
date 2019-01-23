import { SafePipe } from './safe.pipe';
import { DynamicSortPipe } from './dynamic-sort.pipe';
import { EnumReaderPipe } from './enum-reader.pipe';
import { ReadingTimePipe } from './readingtime.pipe';
import { StripHtmlPipe } from './striphtml.pipe';
import { TruncateTextPipe } from './truncatetext.pipe';

export const pipes = [
  SafePipe,
  DynamicSortPipe,
  EnumReaderPipe,
  ReadingTimePipe,
  StripHtmlPipe,
  TruncateTextPipe,
];
