import { globalSettings } from '../../../utils';
import { Injectable } from '@angular/core';

import { LocalStorageService } from '../../services/local-storage.service';

import { Filter } from '../models';

@Injectable()
export class FiltersStorageService {
  constructor(
    private localStorage: LocalStorageService
  ) { }

  isEnable() {
    return globalSettings.isSaveFiltersInStorage;
  }

  getStorageName(gridName) {
    return gridName + '-filters';
  }

  getFromStorage(gridName): Filter[] {
    if (!this.isEnable()) {
      return null;
    }
    return this.localStorage.get(this.getStorageName(gridName));
  }

  setToStorage(newValue: Filter[], gridName) {
    if (this.isEnable()) {
      this.localStorage.set(this.getStorageName(gridName), newValue);
    }
  }
}
