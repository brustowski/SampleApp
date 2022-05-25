import { InboundRecordParameter } from '../models';

export interface ImportRecordParameterValidator {
  validate(parameter: InboundRecordParameter): string | null;
}
