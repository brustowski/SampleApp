import { TestBed } from '@angular/core/testing';

import { FilingParametersService } from './filing-parameters.service';

describe('FilingParametersService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FilingParametersService = TestBed.get(FilingParametersService);
    expect(service).toBeTruthy();
  });
});
