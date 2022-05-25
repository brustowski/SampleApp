import { TestBed } from '@angular/core/testing';

import { AuditApiService } from './audit-api.service';

describe('AuditApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AuditApiService = TestBed.get(AuditApiService);
    expect(service).toBeTruthy();
  });
});
