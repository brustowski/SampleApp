import { Injectable } from '@angular/core';
import * as R from 'ramda';

import { Filter, FilterBuilder, FilterServer } from '../models';

@Injectable()
export class FilterService {
  public filters: Array<Filter> = [];

  constructor() { }

  getFilters() {
    return this.filters;
  }

  setFilters(filters: Filter[]) {
    this.filters = filters;
  }

  getFilterByOperand(operand) {
    if (!operand) {
      return null;
    }

    const filter = R.find(R.propEq('operand', operand))(this.filters);
    if (filter && !filter.options) {
      filter.options = {};
    }

    return filter;
  }

  getFilterByFieldName(fieldName: string): Filter {
    if (!fieldName) {
      return null;
    }

    const filter = R.find(R.propEq('fieldName', fieldName))(this.filters);
    if (filter && !filter.options) {
      filter.options = {};
    }

    return filter;
  }

  setActiveFilters(activeFilters: Filter[]) {
    const setStorageValue = (filterItem) => {
      let filter = null;
      if (filterItem.type === 'date') {
        filter = this.getFilterByOperand(filterItem.operand);
      } else {
        filter = this.getFilterByFieldName(filterItem.fieldName);
      }

      if (filter) {
        filter.value = filterItem.value;
      }
    };

    if (activeFilters) {
      R.forEach(setStorageValue, activeFilters);
    }
  }

  parseFilters(gridName: string, item: FilterServer): Filter {
    return new FilterBuilder().create()
      .title(item.Title)
      .fieldName(item.FieldName)
      .gridName(gridName)
      .type(item.Type)
      .operand(item.Operand)
      .dependOn(item.DependOn)
      .isSearch(item.IsSearch)
      .showTopList(!item.IsSearch)
      .maxLength(item.MaxLength)
      .advanced(item.Advanced)
      .isUpdateFilter(item.IsUpdateFilter)
      .isMultiSelect(item.IsMultiSelect)
      .build();
  }

}
