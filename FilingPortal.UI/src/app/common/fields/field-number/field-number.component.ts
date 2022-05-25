import { Component, OnInit, Input } from '@angular/core';

import * as R from 'ramda';

import { FieldOptions } from '../models/FieldOptions';
import { RequiredField } from '../field-required-ctrl';

@Component({
  selector: 'lxft-field-number',
  templateUrl: './field-number.component.html'
})
export class FieldNumberComponent extends RequiredField implements OnInit {
  @Input() options: FieldOptions = new FieldOptions();
  @Input() value: any = null;

  private numberPatternError =
    'Incorrect value. Only Number (decimals are supported) values are allowed';
  private regexNumberPattern = new RegExp('^[+-]?[0-9]+$');

  constructor() {
    super();
  }

  ngOnInit() {
    if (this.viewMode) {
      return;
    }
    this.check(this.value);
  }

  onChange($event: any) {
    super.onChange($event);
    this.checkFormat($event.target.value);
  }

  check(value: any) {
    this.checkRequired(value);
    this.checkFormat(value);
  }

  checkFormat(value: any) {
    const indexError = this.options.errors.indexOf(this.numberPatternError);

    // null, undefined or empty string are correct values
    if (R.isNil(value) || R.isEmpty(value)) {
      if (indexError >= 0) {
        this.options.errors.splice(indexError, 1);
      }
      return;
    }

    if (this.regexNumberPattern.test(value) && indexError >= 0) {
      this.options.errors.splice(indexError, 1);
      return;
    }
    if (!this.regexNumberPattern.test(value) && indexError < 0) {
      this.options.errors.push(this.numberPatternError);
    }
  }
}
