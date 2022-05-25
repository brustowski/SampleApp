import { Injectable } from '@angular/core';

import { FiltersStorageService } from '@common/filters/services/filters-storage.service';

import { Page, Sort } from '../models/page';
import { PageParams } from '../models/page-params';
import { Filter } from '@common/filters/models';

@Injectable()
export class GridService {
    constructor(
        private filtersStorage: FiltersStorageService
    ) {}

    getPageParams(paginationOptions: Page, sortOptions: Sort, filtersOptions: Filter[]): PageParams {
        const filters = this.filtersStorage.getFromStorage(paginationOptions.filterConfigName) || filtersOptions;

        return new PageParams(paginationOptions, sortOptions, filters);
    }
}
