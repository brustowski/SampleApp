import { TestBed } from '@angular/core/testing';

import { InboundTypeWatcherService } from './inbound-type-watcher.service';

describe('InboundTypeWatcherService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InboundTypeWatcherService = TestBed.get(InboundTypeWatcherService);
    expect(service).toBeTruthy();
  });
});
