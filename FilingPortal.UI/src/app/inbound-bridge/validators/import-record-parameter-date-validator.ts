import { ImportRecordParameterValidator } from './import-record-parameter-validator';
import { ValueValidator } from '@common/validators';
import { InboundRecordParameter } from '@inbound/models';

export class ImportRecordParameterDateValidator
  implements ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null {
    if (parameter.type === 'Date' && !ValueValidator.date(parameter.value)) {
      return `Field ${
        parameter.title
      } contains incorrect value. Only Date values are allowed`;
    }
    return null;
  }
}
