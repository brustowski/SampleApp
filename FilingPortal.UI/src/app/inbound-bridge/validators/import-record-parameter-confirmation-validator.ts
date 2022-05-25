import { InboundRecordParameter } from '../models';
import { ImportRecordParameterValidator } from './import-record-parameter-validator';

export class ImportRecordParameterConfirmationValidator
  implements ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null {
    if (
      parameter.additionalFields.length > 1 
      && parameter.additionalFields[0].confirmationNeeded
      && !(parameter.additionalFields[1] && parameter.additionalFields[1].value)
    ) {
      return `Confirmation required for field ${parameter.title}`;
    }
    return null;
  }
}
