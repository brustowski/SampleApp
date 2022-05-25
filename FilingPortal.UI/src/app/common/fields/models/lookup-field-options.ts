import { FieldOptions, FieldOptionsBuilder } from './FieldOptions';
import { ListOptionModel } from './ListOptionModel';
import { LookupFieldSource } from './lookup-field-source';

export class LookupFieldOptions extends FieldOptions {
  options: Array<ListOptionModel> = [];
  source: LookupFieldSource;
  searchable: boolean;
  isSearchable: boolean = false;
  isClearable: boolean = false;
  canAdd: boolean = false;
}

export class LookupFieldOptionsBuilder extends FieldOptionsBuilder {
  model: LookupFieldOptions;

  create() {
    this.model = new LookupFieldOptions();
    return this;
  }

  searchable(value: boolean) {
    this.model.isSearchable = value;
    return this;
  }

  clearable() {
    this.model.isClearable = true;
    return this;
  }

  source(source: LookupFieldSource) {
    this.model.source = source;
    return this;
  }

  canAdd(value: boolean) {
    this.model.canAdd = value;
    return this;
  }

  options(value: Array<ListOptionModel>): LookupFieldOptionsBuilder {
    this.model.options = value;
    return this;
  }
}
