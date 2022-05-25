import { TestBed, inject } from '@angular/core/testing';

import { RulesApiService } from './rules-api.service';

describe('RulesApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RulesApiService]
    });
  });

  it('should be created', inject([RulesApiService], (service: RulesApiService) => {
    expect(service).toBeTruthy();
  }));
});
