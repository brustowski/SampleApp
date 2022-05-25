import { TestBed } from '@angular/core/testing';

import { ReconService } from './recon.service';

describe('ReconService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReconService  = TestBed.get(ReconService);
    expect(service).toBeTruthy();
  });
});
