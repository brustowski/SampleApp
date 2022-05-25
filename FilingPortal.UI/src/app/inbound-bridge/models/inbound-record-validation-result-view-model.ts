import { ValidationResultViewModel } from '@common/models';
import { InboundRecordErrorViewModel } from './inbound-record-error-view-model';
import { InboundRecordListActions } from './inbound-record-list-actions';

export class InboundRecordValidationResultViewModel implements ValidationResultViewModel {
    IsValid: boolean;
    CommonError: string;
    RecordErrors: InboundRecordErrorViewModel[] = [];
    Actions: InboundRecordListActions = new InboundRecordListActions();
  }
