import { InboundRecordParameter } from '@inbound/models';

/**
 * Form configuration class
 */
export class FormConfig {
    /** record id */
    id?: number;
    /** Form fields */
    fields: InboundRecordParameter[];
}
