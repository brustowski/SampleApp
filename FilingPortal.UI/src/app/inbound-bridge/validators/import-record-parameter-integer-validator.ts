import { ImportRecordParameterValidator } from './import-record-parameter-validator';
import { ValueValidator } from '@common/validators';
import { InboundRecordParameter } from '@inbound/models';

export class ImportRecordParameterIntegerValidator
  implements ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null {
    if (
      parameter.type === 'Integer' &&
      !ValueValidator.integer(parameter.value)
    ) {
      return `Field ${
        parameter.title
      } contains incorrect value. Only Integer values are allowed`;
    }
    return null;
  }
}
