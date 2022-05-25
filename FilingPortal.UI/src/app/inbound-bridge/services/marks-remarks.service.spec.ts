import { TestBed } from '@angular/core/testing';

import { MarksRemarksService } from './marks-remarks.service';

describe('MarksRemarksService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MarksRemarksService = TestBed.get(MarksRemarksService);
    expect(service).toBeTruthy();
  });
});
