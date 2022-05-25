import * as R from 'ramda';
import { Component, Input, EventEmitter, OnInit, OnDestroy } from '@angular/core';

import { FiltersApiService } from '../services/filters-api.service';
import { FilterService } from '../services/filter.service';

import { Filter, FilterSearchSettings, FilterSearchSettingsBuilder } from '../models';

import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';
import { Subject, Subscription } from 'rxjs';

@Component({
  selector: 'lxft-filter-select',
  templateUrl: './filter-select.component.html'
})
export class FilterSelectComponent implements OnInit, OnDestroy {
  subscription: Subscription;

  @Input() filter: Filter;
  @Input() set commandSubject(subj: Subject<string>) {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    this.subscription = subj.subscribe(x => {
      if (x === 'clear') {
        this.clear();
      }
    });
  }

  typeahead = new EventEmitter<string>();

  constructor(
    private _api: FiltersApiService,
    private _filtersService: FilterService,
  ) { }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  ngOnInit() {
    this.typeahead
      .distinctUntilChanged()
      .debounceTime(400)
      .subscribe(this.refresh.bind(this, this.filter));

    this.refresh(this.filter);
  }

  onChangeSelect(field: Filter) {
    this.resetDependFilter(field);
  }

  refresh(filterItem: Filter, searchData: string = '') {
    if (filterItem.disableBackendSearch) {
      return;
    }

    this.getOptionsForFilterItem(searchData, filterItem);
  }

  getOptionsForFilterItem(searchData: string, filterItem: Filter) {
    if (!searchData && filterItem.search && !filterItem.isShowTopList) {
      filterItem.options = filterItem.isMultiSelect && filterItem.value && filterItem.value && filterItem.value.length
        ? [...filterItem.value]
        : [];
      return;
    }

    this._api.getOptions(this.getSettingsForGetOptions(filterItem, searchData))
      .subscribe((data) => {
        filterItem.options = filterItem.isMultiSelect && filterItem.value && filterItem.value && filterItem.value.length
          ? [...filterItem.value, ...data]
          : data;

        this.setDefaultValue(filterItem);
      });
  }

  getSettingsForGetOptions(filterItem: Filter, searchData: string): FilterSearchSettings {
    const dependField = this._filtersService.getFilterByFieldName(filterItem.dependOn);

    return new FilterSearchSettingsBuilder().create()
      .gridName(filterItem.gridName)
      .fieldName(filterItem.fieldName)
      .searchString(searchData)
      .dependOn(filterItem.dependOn)
      .dependValue(dependField && dependField.value ? dependField.value.Value : '')
      .build();
  }

  resetDependFilter(filter: Filter) {
    const filters = this._filtersService.getFilters();

    const filterRelated = R.find(R.propEq('dependOn', filter.fieldName))(filters);
    if (filterRelated) {
      filterRelated.options = [];
      filterRelated.value = null;
      this.refresh(filterRelated, '');
    }
  }

  setDefaultValue(filterItem: Filter) {
    const defaultOption = R.find(o => R.isNil(o.Value), filterItem.options);
    if (R.isNil(defaultOption)) {
      return;
    }
    filterItem.defaultValue = defaultOption;
    if (R.isNil(filterItem.value)) {
      filterItem.value = filterItem.defaultValue;
    }
  }

  getSelectPlaceholder(isSearch: boolean) {
    return isSearch ? 'Type to search' : 'Select';
  }

  clear() {
    this.filter.value = null;
    this.refresh(this.filter);
  }
}
