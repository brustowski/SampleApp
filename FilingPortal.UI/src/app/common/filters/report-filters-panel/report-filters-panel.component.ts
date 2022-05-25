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

import { Subscription, Subject } from 'rxjs';
import * as R from 'ramda';

import { KeyCodes } from '@common/models/key-codes.enum';
import { Filter } from '@common/filters/models';
import { FiltersApiService, FilterService, FiltersStorageService } from '@common/filters/services';
import { EventsService } from '@common/services';

@Component({
  selector: 'lxft-report-filters-panel',
  templateUrl: './report-filters-panel.component.html'
})
export class ReportFiltersPanelComponent implements OnInit, OnDestroy {
  public isLoading: boolean = false;
  public isCollapsed: boolean = true;
  public isAdvancedCollapsed: boolean = true;
  public filters: Array<Filter>;
  public activeFilters: Array<Filter> = [];
  public commandSubject: Subject<string> = new Subject();

  @Output() onReport: EventEmitter<any> = new EventEmitter();
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
  report() {
    const filtersWithValue = R.filter(
      f =>
        R.prop('value', f) && R.prop('value', f) !== R.prop('defaultValue', f),
      this.filters
    );
    this.setActiveFilters(filtersWithValue);
    this.onReport.emit(this.activeFilters);
  }

  setActiveFilters(newValue) {
    this.activeFilters = newValue;
    this.filtersStorage.setToStorage(this.activeFilters, this.gridName);
  }

  resetFilters() {
    this.filters.forEach(item => (item.value = item.defaultValue));
    this.commandSubject.next('clear');

    this.setActiveFilters(null);
    this.onReset.emit();
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
