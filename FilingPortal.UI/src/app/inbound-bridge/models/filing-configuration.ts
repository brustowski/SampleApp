import { InboundRecordDocument } from './inbound-record-document';
import { FilingConfigurationField } from './filing-configuration-field';
import { FilingConfigurationSection } from './filing-configuration-section';

export class FilingConfiguration {
  filingHeaderId: number;
  fields: FilingConfigurationField[] = [];
  sections: FilingConfigurationSection[] = [];
  documents: InboundRecordDocument[] = [];
}
