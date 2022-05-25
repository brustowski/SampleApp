import { TestBed } from '@angular/core/testing';
import { LocalStorageService } from '../../services/local-storage.service';
import { GridStorageService } from './grid-storage.service';
import { Sort } from '../models/page';

class MockStorageService {
  private value: any;
  private key: string;

  set(key: string, val: any) {
    this.key = key;
    this.value = val;
  }

  get(key: string): any {
    if (this.key === key) {
      return this.value;
    }
    return null;
  }

  clearAll() {
    this.key = '';
    this.value = null;
  }
}

describe('GridStorageService', () => {
  let service: GridStorageService;
  let storageService: MockStorageService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GridStorageService,
        { provide: LocalStorageService, useClass: MockStorageService }
      ]
    });
    service = TestBed.get(GridStorageService);
    storageService = TestBed.get(LocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be enabled', () => {
    expect(service.isEnable).toEqual(true);
  });

  it('should call storage with correct name when getting value from storage', () => {
    storageService.clearAll();
    const gridName = 'test-grid';
    const spy = spyOn(storageService, 'get');
    service.getSortingFromStorage(gridName);
    expect(spy).toHaveBeenCalledTimes(1);
  });

  it('should call storage with correct name when setting value to storage', () => {
    storageService.clearAll();
    const gridName = 'test';
    const settings = <Sort>{ column: 'first', order: 'asc'};
    const spy = spyOn(storageService, 'set');
    service.setSorting(settings, gridName);
    expect(spy).toHaveBeenCalledTimes(1);
  });

  it('should set sorting settings in storage', () => {
    storageService.clearAll();
    const gridName = 'test';
    const settings = <Sort>{ column: 'first', order: 'asc'};
    service.setSorting(settings, gridName);

    expect(storageService.get('test-grid-sorting')).toEqual(settings);
  });
});
