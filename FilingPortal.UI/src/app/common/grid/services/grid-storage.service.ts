import { globalSettings } from '../../../utils';
import { Injectable } from '@angular/core';

import { LocalStorageService } from '../../services/local-storage.service';

import { Sort } from '../models/page';
import { GridConfiguration, ColumnConfiguration } from '../models';

enum GridStorageSettingsType {
  sorting,
  pageSize,
  columnSizes
}

@Injectable()
export class GridStorageService {
  constructor(
    private localStorage: LocalStorageService
  ) { }

  get isEnable(): boolean {
    return globalSettings.isSaveGridSettingsInStorage;
  }

  getSortingFromStorage(gridName: string): Sort {
    const config = this.getConfiguration(gridName);
    return config ? config.sorting : null;
  }

  setSorting(value: Sort, gridName: string): void {
    this.setConfiguration(gridName, { ...this.getConfiguration(gridName), sorting: value });
  }

  setPageSize(value: number, gridName: string): void {
    this.setConfiguration(gridName, { ...this.getConfiguration(gridName), pageSize: value });
  }

  setColumnSize(columnName: string, size: number, gridName: string) {
    const config = this.getConfiguration(gridName);
    if (config) {
      if (!config.columns) {
        config.columns = [];
      }
      const indx = config.columns.findIndex(x => x.prop === columnName);
      if (indx !== -1) {
        config.columns[indx].width = size;
      } else {
        config.columns.push(<ColumnConfiguration>{ prop: columnName, isVisible: true, width: size });
      }
      this.setConfiguration(gridName, config);
    }
  }

  setColumnConfiguration(gridName: string, columns: ColumnConfiguration[]): void {
    const configuration = this.getConfiguration(gridName);
    if (configuration) {
      configuration.columns = columns;
      this.setConfiguration(gridName, configuration);
    }
  }

  getConfiguration(gridName: string): GridConfiguration {
    return this.isEnable ? this.localStorage.get(gridName) || new GridConfiguration() : null;
  }

  setConfiguration(gridName: string, configuration: GridConfiguration): void {
    if (this.isEnable) {
      this.localStorage.set(gridName, configuration);
    }
  }
}
