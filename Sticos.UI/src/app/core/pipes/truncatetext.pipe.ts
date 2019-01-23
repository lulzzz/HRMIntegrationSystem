import { Pipe, PipeTransform } from '@angular/core';
/*
 * Truncates an text with defined cut off length
 * Takes an input parameter string.
 * Usage:
 *   string | truncatetext: 100
 * Example:
 *   <p>{{ string | truncatetext: 100 }}></p>
*/
@Pipe({
  name: 'truncatetext',
})
export class TruncateTextPipe implements PipeTransform {
  transform(value: string, length: number): string {
    const biggestWord = 50;
    const elipses = ' ...';

    if (typeof value === 'undefined') {
      return value;
    }
    if (value.length <= length) {
      return value;
    }

    // .. truncate to about correct lenght
    let truncatedText = value.slice(0, length + biggestWord);

    // .. now nibble ends till correct length
    while (truncatedText.length > length - elipses.length) {
      const lastSpace = truncatedText.lastIndexOf(' ');

      if (lastSpace === -1) {
        break;
      }

      truncatedText = truncatedText
        .slice(0, lastSpace)
        .replace(/[!,.?;:]$/, '');
    }

    return truncatedText + elipses;
  }
}
