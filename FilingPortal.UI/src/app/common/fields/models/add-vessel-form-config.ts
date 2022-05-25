import { InboundRecordParameter } from '@inbound/models';

/**
 * Add new vessel form configuration class
 */
export class AddVesselFormConfig {
    /** record id */
    id?: number;
    /** Form fields */
    fields: InboundRecordParameter[];
}
