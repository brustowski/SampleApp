import { Component, OnInit, Input, forwardRef, ViewChild, OnChanges, SimpleChanges } from '@angular/core';

import { Subject } from 'rxjs/Subject';
import * as R from 'ramda';
import { NgSelectComponent } from '@ng-select/ng-select';

import { RequiredField } from '../field-required-ctrl';
import { FieldsApiService } from '../services/fields-api.service';
import { LookupSearchSettings } from '../models/lookup-search-settings';
import { FieldsService } from '../services/fields.service';
import { ListOptionModel } from '../models/ListOptionModel';
import { isUndefined, isString } from 'util';
import { Observable, Subscription } from 'rxjs';


@Component({
  selector: 'lxft-field-lookup',
  templateUrl: './field-lookup.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldLookupComponent) }]
})
export class FieldLookupComponent extends RequiredField implements OnInit, OnChanges {

  @ViewChild('select') select: NgSelectComponent;

  private _placeholder: string;
  private isInitialLoading: boolean = true;
  public loaded: boolean = false;

  @Input() set placeholder(value: string) {
    this._placeholder = value;
  }
  get placeholder(): string {
    const value = this._placeholder ? this._placeholder
      : this.options && this.options.isSearchable
        ? 'Type to search' : 'Select';
    return value;
  }

  @Input() dependOnValue: string;

  typeahead$: Subject<string> = new Subject<string>();
  isLoading: boolean = false;
  values: Array<ListOptionModel> = [];
  displayValue: string = '';

  constructor(private apiService: FieldsApiService, private fieldsService: FieldsService) {
    super();
  }

  ngOnChanges(): void {
    if (this.options) {
      this.loadOptions();
    }
  }

  ngOnInit() {
    if (this.viewMode) {
      const subscription = this.getDisplayValue(this.value)
        .subscribe(result => {
          if (result && result.Value === this.value) {
            this.displayValue = result.DisplayValue;
          }
          subscription.unsubscribe();

        });
    }
    this.loaded = true;
    if (this._options) {
      this.loadOptions();
    }
  }

  loadOptions() {
    if (this.loaded) {
      this.options.isSearchable = true;

      if (!this.trySetLocalValues()) {
        this.trySetTypeaheadSubscription();
        if (R.isNil(this.value) || R.isEmpty(this.value)) {
          this.loadData();
        } else {
          this.loadData(this.value, true);
        }
      }
    }
  }

  private trySetLocalValues(): boolean {
    this.values = this.options.options && this.options.options.length > 0 ? this.options.options : [];
    return this.values.length > 0;
  }

  private trySetTypeaheadSubscription(): boolean {
    if (this.options.isSearchable) {
      this.typeahead$
        .debounceTime(400)
        .distinctUntilChanged()
        .subscribe(this.loadData.bind(this));
    }
    return this.options.isSearchable;
  }

  isClearable(): boolean {
    //  make it clearable only when search is enabled
    return this.options && this.options.isSearchable;
  }

  getValue() {
    return this.value;
  }

  getDisplayValue(searchData: string = ''): Observable<ListOptionModel> {
    return this.downloadOptions(searchData, true).map(x => x.find(v => v.Value === searchData));
  }

  downloadOptions(searchData: string = '', searchByKey: boolean = false): Observable<ListOptionModel[]> {
    if (this.options.source) {
      const searchSettings = new LookupSearchSettings();
      searchSettings.fieldName = this.options.source.propertyName
        ? this.options.source.propertyName
        : this.options.name;
      searchSettings.sourceName = this.options.source.name;
      searchSettings.sourceType = this.options.source.type;
      searchSettings.limit = this.options.isSearchable ? 20 : 0;
      searchSettings.dependValue = this.dependOnValue ? this.dependOnValue : '';
      searchSettings.searchText = searchData;
      searchSettings.searchByKey = searchByKey;
      if (this.options.dependsOn) {
        searchSettings.dependOn = this.options.dependsOn;
        searchSettings.dependValue = this.fieldsService.getFieldValue(this.options.dependsOn);
      }
      return this.apiService.getLookupFieldValue(searchSettings);
    }
  }

  loadData(searchData: string = '', searchByKey: boolean = false): void {
    this.isLoading = true;
    this.downloadOptions(searchData, searchByKey).subscribe(
      result => {
        this.values = result;
        this.isLoading = false;
        this.updateValue();
      },
      () => {
        this.isLoading = false;
        this.values = [];
      }
    );
  }

  clear(systemClear: boolean = false) {
    if (this.isClearable) {
      if (systemClear) {
        this.isInitialLoading = true;
      }
      super.clear();
      this.loadData();
    }
  }

  private updateValue() {
    if (this.isInitialLoading && this.values && this.values.some(x => x.IsDefault)) {
      this.value = this.values.find(x => x.IsDefault).Value;
      super.onChange(null);
      this.isInitialLoading = false;
      return;
    }
    const handbookValue = this.values
      ? this.values.find(v => `${v.Value}`.trim().toUpperCase() === `${this.value}`.trim().toUpperCase())
      : undefined;
    if (handbookValue !== undefined) {
      this.value = handbookValue.Value;
    } else {
      this.value = undefined;
    }
  }

  onChange($event) {
    const textValue = isString($event) || R.isNil($event) ? $event : $event.DisplayValue;

    this.fieldsService.setDisplayValue(this.options.name, textValue);

    if ($event && isUndefined($event.Value)) {
      // We have a new value here
      // Event may be a plain string
      let dependValue = null;
      if (this.options.dependsOn) {
        dependValue = this.fieldsService.getFieldValue(this.options.dependsOn);
      }
      this.apiService.addOption(textValue, this.options.source.name, dependValue).subscribe(x => {
        this.values = [x];
        this.value = x.Value;
        this.updateValue();
        super.onChange(null);
      });
    } else {
      super.onChange(null);
    }
  }

  public refresh(): void {
    this.loadOptions();
    if (this.select.dropdownPanel) {
      this.select.dropdownPanel.refresh();
    }
  }
}
