import { TestBed } from '@angular/core/testing';

import { DocDownloadService } from './doc-download.service';

describe('PipelineDocGeneratorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DocDownloadService = TestBed.get(DocDownloadService);
    expect(service).toBeTruthy();
  });
});
