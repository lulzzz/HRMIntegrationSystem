import { TestBed, inject } from '@angular/core/testing';

import { MappingCommunicationService } from './mapping-communication.service';

describe('MappingCommunicationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MappingCommunicationService],
    });
  });

  it('should be created', inject(
    [MappingCommunicationService],
    (service: MappingCommunicationService) => {
      expect(service).toBeTruthy();
    },
  ));
});
