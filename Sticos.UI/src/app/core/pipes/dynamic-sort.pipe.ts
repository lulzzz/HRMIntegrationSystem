import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sortDynamic',
})
export class DynamicSortPipe implements PipeTransform {
  transform(array: Array<any>, param: string): any {
    if (array) {
      let orderByValue = param;
      let byVal = 1;

      if (orderByValue.charAt(0) === '!') {
        byVal = -1;
        orderByValue = orderByValue.substring(1);
      }

      array.sort((a: any, b: any) => {
        if (a[orderByValue] < b[orderByValue]) {
          return 1 * byVal;
        } else if (a[orderByValue] > b[orderByValue]) {
          return -1 * byVal;
        } else {
          return 0;
        }
      });
      return array;
    }
  }
}
