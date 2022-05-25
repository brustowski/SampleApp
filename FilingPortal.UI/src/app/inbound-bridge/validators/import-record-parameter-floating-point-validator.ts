import { ValueValidator } from '@common/validators';
import { InboundRecordParameter } from '@inbound/models';
import { ImportRecordParameterValidator } from './import-record-parameter-validator';

export class ImportRecordParameterFloatingPointValidator
  implements ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null {
    if (
      parameter.type === 'Number' &&
      !ValueValidator.floatingPointNumber(parameter.value)
    ) {
      return `Field ${
        parameter.title
      } contains incorrect value. Only Floating Point Number values are allowed`;
    }
    return null;
  }
}
