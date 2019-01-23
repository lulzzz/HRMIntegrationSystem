import { TestBed, inject } from '@angular/core/testing';

import { FilePreviewUrlService } from './filepreviewurl.service';

describe('FilepreviewurlService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FilePreviewUrlService],
    });
  });

  it('should be created', inject(
    [FilePreviewUrlService],
    (service: FilePreviewUrlService) => {
      expect(service).toBeTruthy();
    },
  ));
});
