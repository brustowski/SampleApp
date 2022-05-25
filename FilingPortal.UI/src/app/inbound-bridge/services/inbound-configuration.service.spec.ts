import { TestBed, inject } from '@angular/core/testing';

import { InboundConfigurationService } from './inbound-configuration.service';

describe('InboundMappingsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InboundConfigurationService]
    });
  });

  it('should be created', inject([InboundConfigurationService], (service: InboundConfigurationService) => {
    expect(service).toBeTruthy();
  }));
});
