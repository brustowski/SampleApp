import { TestBed } from '@angular/core/testing';
import { LocalStorageService } from '../../services/local-storage.service';
import { FiltersStorageService } from './filters-storage.service';
import { Filter } from '../models';

class MockStorageService {
  private filters: Filter[] = [];
  private key: string = '';

  set(key, val) {
    this.key = key;
    this.filters = val;
  }

  get(key): any {
    if (this.key === key) {
      return this.filters;
    }
    return null;
  }

  clearAll() {
    this.key = '';
    this.filters = [];
  }
}

describe('FiltersStorageService', () => {
  let service: FiltersStorageService;
  let storageService: MockStorageService;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FiltersStorageService,
        { provide: LocalStorageService, useClass: MockStorageService }
      ]
    });
    service = TestBed.get(FiltersStorageService);
    storageService = TestBed.get(LocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be enabled', () => {
    expect(service.isEnable()).toEqual(true);
  });

  it('should call storage with correct name when getting value from storage', () => {
    storageService.clearAll();
    const gridName = 'test-grid';
    const spy = spyOn(storageService, 'get');
    service.getFromStorage(gridName);
    expect(spy).toHaveBeenCalledTimes(1);
  });

  it('should call storage with correct name when setting value to storage', () => {
    storageService.clearAll();
    const gridName = 'test-grid';
    const filters: Filter[] = [];
    const spy = spyOn(storageService, 'set');
    service.setToStorage(filters, gridName);
    expect(spy).toHaveBeenCalledTimes(1);
  });

  it('should set filters in storage', () => {
    storageService.clearAll();
    const gridName = 'test-grid';
    const filters: Filter[] = [new Filter()];
    service.setToStorage(filters, gridName);

    expect(storageService.get('test-grid-filters')).toEqual(filters);
  });
});
