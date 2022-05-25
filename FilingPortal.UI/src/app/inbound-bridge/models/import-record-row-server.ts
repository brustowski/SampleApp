import { ComplexInboundRecordField, InboundRecordField } from '@common/models';

export interface ImportRecordRowServer {
  FilingHeaderId: number;
  RecordId: number;
  Parameters: (InboundRecordField | ComplexInboundRecordField)[];
}
