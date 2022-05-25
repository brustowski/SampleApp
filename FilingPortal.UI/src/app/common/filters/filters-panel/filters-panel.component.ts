import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  HostListener,
  OnDestroy
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'rxjs';
import * as R from 'ramda';

import { KeyCodes } from '@common/models/key-codes.enum';
import { EventsService } from '@common/services/events.service';
import { FiltersApiService, FilterService, FiltersStorageService } from '../services';
import { Filter } from '../models';

@Component({
  selector: 'lxft-filters-panel',
  templateUrl: './filters-panel.component.html'
})
export class FiltersPanelComponent implements OnInit, OnDestroy {
  public isLoading: boolean = false;
  public isCollapsed: boolean = true;
  public isAdvancedCollapsed: boolean = true;
  public filters: Array<Filter>;
  public activeFilters: Array<Filter> = [];

  @Output() onApply: EventEmitter<any> = new EventEmitter();
  @Output() onReset: EventEmitter<any> = new EventEmitter();

  @Input() disabled = false;

  private nameOfTheGrid: string;
  paramSubscription: Subscription;
  get gridName(): string {
    return this.nameOfTheGrid;
  }
  @Input()
  set gridName(val: string) {
    this.nameOfTheGrid = val;
    this.getFilterConfig();
  }

  @HostListener('window:keydown', ['$event'])
  keyEvent($event: KeyboardEvent) {
    if ($event.keyCode === KeyCodes.Enter && !this.disabled) {
      this.setFilters();
      $event.preventDefault();
      $event.stopPropagation();
    }
  }

  constructor(
    private api: FiltersApiService,
    private filtersService: FilterService,
    private filtersStorage: FiltersStorageService,
    private events: EventsService,
    private route: ActivatedRoute
  ) { }

  public ngOnInit() { }

  ngOnDestroy(): void {
    if (this.paramSubscription) {
      this.paramSubscription.unsubscribe();
      this.paramSubscription = undefined;
    }
  }

  getFilterConfig() {
    const gridName = this.gridName;
    this.api.getFiltersConfig(gridName).subscribe(data => {
      const filters: any = data.map(
        this.filtersService.parseFilters.bind(this, gridName)
      );
      this.filtersService.setFilters(filters);
      this.filters = this.filtersService.getFilters();

      this.setActiveFiltersFromStorage();
      this.overwriteFilters();
    });
  }

  setActiveFiltersFromStorage() {
    this.activeFilters = this.filtersStorage.getFromStorage(this.gridName);
    if (this.activeFilters) {
      this.filtersService.setActiveFilters(this.activeFilters);
    }
  }

  setFilters() {
    const filtersWithValue = R.filter(
      f =>
        R.prop('value', f) && R.prop('value', f) !== R.prop('defaultValue', f),
      this.filters
    );
    this.setActiveFilters(filtersWithValue);
    this.onApply.emit(this.activeFilters);
  }

  setFilterByFieldName(fieldName: string, value: string): void {
    const filter: Filter = R.find(R.propEq('fieldName', fieldName))(this.filters);
    switch (filter.type) {
      case 'date-range':
        const date = new Date(value);
        filter.value = {
          beginDate: { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() },
          endDate: { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() }
        };
        break;
      case 'select':
        filter.value = { DisplayValue: value, Value: value, IsDefault: false };
        break;
      default:
        filter.value = value;
        break;
    }

  }

  setActiveFilters(newValue: Filter[]) {
    this.activeFilters = newValue;
    this.filtersStorage.setToStorage(this.activeFilters, this.gridName);
  }

  resetFilters(noEmit: boolean = false) {
    this.filters.forEach(item => (item.value = item.defaultValue));

    this.setActiveFilters(null);
    if (!noEmit) {
      this.onReset.emit();
    }
  }

  isSubmitDisabled() {
    return this.isLoading;
  }

  isResetDisabled() {
    return this.isLoading || !this.activeFilters;
  }

  getFilterByFieldName(fieldName) {
    return this.filtersService.getFilterByFieldName(fieldName);
  }

  getFilterByOperand(operand) {
    return this.filtersService.getFilterByOperand(operand);
  }

  toggleCollapsed() {
    this.isCollapsed = !this.isCollapsed;
    this.events.updateGridSize$.emit();
  }

  clearFilterItem(filter) {
    filter.value = null;
  }

  clearFilterItemByFieldName(fieldName: string): void {
    const filter: Filter = R.find(R.propEq('fieldName', fieldName))(this.filters);
    filter.value = filter.defaultValue;
  }

  isClearItemDisabled(filter) {
    return !filter || !filter.value;
  }

  overwriteFilters() {
    this.paramSubscription = this.route.params.subscribe(params => {
      const filterKeys = Object.keys(params).filter(x => x.startsWith('filter-'));
      if (filterKeys.length) {
        filterKeys.forEach(key => {
          const fieldName = key.substring('filter-'.length);
          const value = params[key];
          const filter = this.filters.find(x => x.fieldName === fieldName);
          if (filter) {
            filter.value = value;
          }
        });
        this.setFilters();
      }
    });
  }

  public toggleAdvancedCollapsed(): void {
    this.isAdvancedCollapsed = !this.isAdvancedCollapsed;
    this.events.updateGridSize$.emit();
  }
}
