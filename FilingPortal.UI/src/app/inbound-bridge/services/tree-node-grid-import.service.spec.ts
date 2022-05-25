import { TestBed } from '@angular/core/testing';

import { TreeNodeGridImportService } from './tree-node-grid-import.service';

describe('TreeNodeGridImportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TreeNodeGridImportService = TestBed.get(TreeNodeGridImportService);
    expect(service).toBeTruthy();
  });
});
