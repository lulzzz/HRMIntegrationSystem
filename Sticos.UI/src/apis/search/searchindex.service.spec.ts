import { TestBed, inject } from '@angular/core/testing';

import { SearchIndexService } from './searchindex.service';

describe('SearchIndexService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SearchIndexService],
    });
  });

  it('should be created', inject(
    [SearchIndexService],
    (service: SearchIndexService) => {
      expect(service).toBeTruthy();
    },
  ));
});
