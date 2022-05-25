import { TestBed, inject } from '@angular/core/testing';

import { InboundRecordsApiService } from './inbound-records-api.service';

describe('InboundApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InboundRecordsApiService]
    });
  });

  it('should be created', inject([InboundRecordsApiService], (service: InboundRecordsApiService) => {
    expect(service).toBeTruthy();
  }));
});
