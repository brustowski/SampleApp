import { TestBed } from '@angular/core/testing';

import { ReconApiService } from './recon-api.service';

describe('AuditApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReconApiService = TestBed.get(ReconApiService);
    expect(service).toBeTruthy();
  });
});
