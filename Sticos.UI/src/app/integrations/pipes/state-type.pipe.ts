import { Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Pipe({
  name: 'stateType',
})
export class StateTypePipe implements PipeTransform {
  result: string;

  constructor(private translateService: TranslateService) {}
  transform(param: any): any {
    const key = `integrations.mapping.statetypemessages.${param}`;
    this.translateService.get(key).subscribe((value: string) => {
      this.result = value;
    });
    return this.result;
  }
}
