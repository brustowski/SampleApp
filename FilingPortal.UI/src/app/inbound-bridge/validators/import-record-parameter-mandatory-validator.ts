import { InboundRecordParameter } from '../models';
import { ImportRecordParameterValidator } from './import-record-parameter-validator';
import { ValueValidator } from '@common/validators';

export class ImportRecordParameterMandatoryValidator
  implements ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null {
    if (
      parameter.options.isMandatory &&
      !ValueValidator.mandatory(parameter.value)
    ) {
      return `Field ${parameter.title} is mandatory`;
    }
    return null;
  }
}
