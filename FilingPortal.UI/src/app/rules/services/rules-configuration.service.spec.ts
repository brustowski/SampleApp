import { TestBed, inject } from '@angular/core/testing';

import { RulesConfigurationService } from './rules-configuration.service';

describe('RulesConfigurationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RulesConfigurationService]
    });
  });

  it('should be created', inject([RulesConfigurationService], (service: RulesConfigurationService) => {
    expect(service).toBeTruthy();
  }));
});
