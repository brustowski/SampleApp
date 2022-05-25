import { Input, Output, EventEmitter } from '@angular/core';
import { FieldOptions } from './models/FieldOptions';

import * as R from 'ramda';
import { FieldBlurEvent } from './models';
import { Subject } from 'rxjs';


export class RequiredField {
  private _viewMode: boolean;
  private _value: any;
  protected _options = new FieldOptions();

  @Input() set options(value: FieldOptions) {
    this._options = value;
    this.onFieldChanged.next();
  }
  get options(): FieldOptions {
    return this._options;
  }
  @Input() get value(): any {
    return this._value;
  }
  set value(value: any) {
    this._value = value;
  }
  @Input() set viewMode(value: boolean) {
    this._viewMode = !!value;
    this.onViewModeChanged();
  }
  get viewMode(): boolean {
    return this._viewMode;
  }

  @Output() valueChange = new EventEmitter<any>();
  @Output() onBlur = new EventEmitter<FieldBlurEvent>();
  @Output() onFieldChanged = new Subject<void>();
  public requiredErrorText: string = 'Field is required';
  protected oldValue = undefined;

  constructor() { }

  onChange(event) {
    this.checkRequired(this.value);
    this.valueChange.emit(this.value);
  }

  checkRequired(value) {
    const errorIndex = this.options.errors.indexOf(this.requiredErrorText);
    if (this.options.isMandatory && (R.isNil(value) || R.isEmpty(value)) && errorIndex < 0) {
      this.options.errors.push(this.requiredErrorText);
      return;
    }
    if ((!this.options.isMandatory || (!R.isNil(value) && !R.isEmpty(value))) && errorIndex >= 0) {
      this.options.errors.splice(errorIndex, 1);
    }
  }

  clear(systemClear: boolean = false): void {
    this.value = undefined;
    this.onChange(this.value);
  }

  onInputBlur(event: FocusEvent) {
    this.onBlur.emit({ event: event, oldValue: this.oldValue });
  }

  onFocus(event: FocusEvent) {
    this.oldValue = this.value || '';
  }

  onViewModeChanged() {  }
}
