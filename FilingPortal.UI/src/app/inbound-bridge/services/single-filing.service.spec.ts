import { TestBed, inject } from '@angular/core/testing';

import { SingleFilingService } from './single-filing.service';

describe('SingleFilingService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SingleFilingService]
    });
  });

  it('should be created', inject([SingleFilingService], (service: SingleFilingService) => {
    expect(service).toBeTruthy();
  }));
});
