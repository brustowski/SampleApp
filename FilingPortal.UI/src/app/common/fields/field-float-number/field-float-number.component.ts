import { Component, OnInit, Input, forwardRef } from '@angular/core';

import { RequiredField } from '../field-required-ctrl';
import { FieldOptions } from '../models/FieldOptions';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'lxft-field-float-number',
  templateUrl: './field-float-number.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldFloatNumberComponent) }]
})
export class FieldFloatNumberComponent extends RequiredField implements OnInit {
  @Input() emptyValue: string = '';
  @Input() get value(): string {
    return(!isNullOrUndefined(this._floatValue)) ? this._floatValue.toString() : this.emptyValue;
  }
  set value(value: string) {
    this._floatValue = !(value === '') && !isNullOrUndefined(value) && !isNaN(+value) ? +value : null;
  }

  _floatValue: number = null;

  constructor() {
    super();
  }

  ngOnInit() {
    if (this.viewMode) {
      return;
    }
    this.check();
  }

  check() {
    this.checkRequired(this.value);
  }
}
