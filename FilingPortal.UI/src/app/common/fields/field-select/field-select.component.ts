import { Component, OnInit, EventEmitter, Input, OnChanges, SimpleChanges, Output } from '@angular/core';
import { FieldsApiService } from '../services/fields-api.service';
import { SelectModel } from '../models/SelectModel';
import { SelectSearchSettings } from '../models/SelectSearchSettingsModel';

@Component({
  selector: 'lxft-field-select',
  templateUrl: './field-select.component.html'
})
export class FieldSelectComponent implements OnInit {
  @Input() model: any = null;
  @Output() modelChange = new EventEmitter<any>();
  @Input() settings: SelectModel;
  @Input() hasErrors: boolean = false;
  @Input() isValidating: boolean = false;
  @Input() onValidate: (value: any) => boolean;

  typeahead = new EventEmitter<string>();

  constructor(private _api: FieldsApiService
  ) { }

  ngOnInit() {
    this.typeahead
      .distinctUntilChanged()
      .debounceTime(400)
      .subscribe(this.refresh.bind(this));

    this.refresh();
    this.validate();
  }

  onChangeSelect() {
    this.validate();
    this.modelChange.emit(this.model);
  }

  validate() {
    if (this.onValidate) {
      this.hasErrors = !this.onValidate(this.model);
    }
  }

  refresh(searchData: string = '') {
    if (!this.settings.isUseBackendOptions) {
      return;
    }

    this.getOptionsForField(searchData);
  }

  getOptionsForField(searchData: string) {
    if (!searchData && !this.settings.isSearch) {
      this.settings.options = [];
      return;
    }

    this._api.getSelectFieldOptions(this.getSettings(searchData))
      .subscribe((data) => {
        this.settings.options = data;
      });
  }

  getSettings(searchData: string): SelectSearchSettings {
    const settings = new SelectSearchSettings();
    settings.dataProviderName = this.settings.providerName;
    settings.search = searchData;
    return settings;
  }

  clear() {
    this.model = null;
    this.refresh();
  }
}
