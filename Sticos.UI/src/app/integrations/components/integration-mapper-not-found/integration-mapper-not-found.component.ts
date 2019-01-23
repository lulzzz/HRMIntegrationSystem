import { Component, Input } from '@angular/core';
import { IntegrationCategoryEnum } from '../../models/integration-category';

@Component({
  templateUrl: './integration-mapper-not-found.component.html',
})
export class IntegrationMapperNotFoundComponent {
  @Input()
  category: string;

  get categoryName() {
    return IntegrationCategoryEnum[this.category];
  }
}
