import { FieldType } from './field-type';

export class FieldOptions {
  [key: string]: any;
  name: string;
  title: string;
  type: FieldType = FieldType.Text;
  isDisabled: boolean = false;
  isMandatory: boolean = false;
  maxLength: number = null;
  dependsOn: string;
  dependsOnValues: string[];
  errors: string[] = [];
  validationOn: boolean = false;
  prefix: string = '';
  uiClass: string = null;

  hasErrors(): boolean {
    return this.errors && this.errors.length > 0;
  }
}

export class FieldOptionsBuilder {
  model: FieldOptions;
  constructor() { }

  create() {
    this.model = new FieldOptions();
    return this;
  }

  name(value: string) {
    this.model.name = value;
    return this;
  }

  title(value: string) {
    this.model.title = value;
    return this;
  }

  fieldType(value: FieldType) {
    this.model.type = value;
    return this;
  }

  maxLength(length = 0) {
    this.model.maxLength = length > 0 ? length : null;
    return this;
  }

  errors(errors = []) {
    this.model.errors = errors;
    return this;
  }

  enableValidation(enable = true) {
    this.model.validationOn = enable;
    return this;
  }

  required(required = false) {
    this.model.isMandatory = required;
    return this;
  }

  disable(disable = false) {
    this.model.isDisabled = disable;
    return this;
  }

  customOption(name: string, value: any) {
    this.model[name] = value;
    return this;
  }

  dependsOn(fieldName: string) {
    this.model.dependsOn = fieldName;
    return this;
  }

  dependsOnValues(values: string[]) {
    this.model.dependsOnValues = values;
    return this;
  }

  prefix(value: string) {
    this.model.prefix = value;
    return this;
  }

  class(value: string) {
    this.model.uiClass = value;
    return this;
  }

  build() {
    return this.model;
  }
}
