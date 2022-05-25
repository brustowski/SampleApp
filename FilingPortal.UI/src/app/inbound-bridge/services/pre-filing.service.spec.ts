import { TestBed } from '@angular/core/testing';

import { PreFilingSevice } from './pre-filing.service';

describe('TruckExportServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PreFilingSevice = TestBed.get(PreFilingSevice);
    expect(service).toBeTruthy();
  });
});
