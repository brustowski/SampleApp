import * as R from 'ramda';
import * as moment from 'moment';

import { dateFormatFull } from '@app/utils';
export class ValueValidator {

  static mandatory(value: any): boolean {
    return !(R.isNil(value) || R.isEmpty(value));
  }

  static integer(value: any): boolean {
    const pattern: RegExp = /^[+-]?[0-9]+$/;
    return R.isNil(value) || R.isEmpty(value)
      ? true
      : pattern.test(value.toString());
  }

  static floatingPointNumber(value: any): boolean {
    const pattern: RegExp = /^[+-]?[0-9]+(\.[0-9]+)?$/;
    return R.isNil(value) || R.isEmpty(value)
      ? true
      : pattern.test(value.toString());
  }

  static date(value: any): boolean {
      if (R.isNil(value) || R.isEmpty(value)) {
        return true;
      }
      const date = moment(value.toString(), dateFormatFull.toUpperCase(), true);
      return date.isValid();
    }
}
