import { Pipe, PipeTransform } from '@angular/core';
import { Data } from '@sticos/apis/timereg';

@Pipe({
  name: 'formatDataset',
})
export class FormatDatasetPipe implements PipeTransform {
  result: string;

  constructor() {}

  transform(array: Array<Data>): string {
    const searchResult = array.map(u => u.value).join(' - ');
    return searchResult;
  }
}
