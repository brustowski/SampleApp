import { BaseInboundRecordField } from '@common/models';

export class FilingConfigurationField {
  id: number;
  filingHeaderId: number;
  recordId: number;
  parentRecordId: number;
  sectionName: string;
  sectionTitle: string;
  order: number;
  isVisibleOn7501: boolean;
  isVisibleOnRuleDrivenData: boolean;
  title: string;
  name: string;
  value: string;
  description: string;
  type: string;
  maxLength: number;
  isMandatory: boolean;
  isDisabled: boolean;
  Field: BaseInboundRecordField;
}
