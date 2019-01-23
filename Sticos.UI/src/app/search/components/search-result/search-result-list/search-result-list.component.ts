import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { SearchResultItem, SearchIndexType } from 'src/apis/search/models';

@Component({
  selector: 'app-search-result-list',
  templateUrl: './search-result-list.component.html',
  styleUrls: ['./search-result-list.component.scss'],
})
export class SearchResultListComponent implements OnInit {
  @Input()
  items: SearchResultItem[] = [];

  constructor() {}

  ngOnInit() {}

  getIcon(indexId: SearchIndexType): string {
    let icon = 'notes';

    if (!indexId) {
      return icon;
    }

    switch (indexId) {
      case SearchIndexType.PersonalInnhold:
        icon = 'address-book';
        break;
      case SearchIndexType.HMSInnhold:
        icon = 'log-book';
        break;
      case SearchIndexType.LederInnhold:
        icon = 'book';
        break;
      case SearchIndexType.Lover:
      case SearchIndexType.Paragrafer:
      case SearchIndexType.Forskrifter:
        icon = 'paragraph-alt';
        break;
      case SearchIndexType.Skjema:
        icon = 'list-alt';
        break;
      case SearchIndexType.Uttalelser:
        icon = 'article';
        break;
      case SearchIndexType.FilArkiv:
        icon = 'file';
        break;
      case SearchIndexType.Google:
        icon = 'globe';
        break;
    }

    return icon;
  }
}
