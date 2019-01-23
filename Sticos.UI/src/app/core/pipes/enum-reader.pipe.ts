import { Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Pipe({
  name: 'enumReader',
})
export class EnumReaderPipe implements PipeTransform {
  result: string;

  constructor(private translateService: TranslateService) {}

  transform(param: string, enumType: string): string {
    const key = `enum.${enumType.toLocaleLowerCase()}.${param}`;
    this.translateService.get(key).subscribe((value: string) => {
      this.result = value;
    });
    return this.result;
  }
}
