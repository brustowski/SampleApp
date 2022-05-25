import { FormConfigServerModel } from '.';

/**
 * Server-side field base class
 */
export class FieldServerModel {
    /** Field name */
    Name: string;
    /** Field title */
    Title: string;
    /** Field value */
    Value: string;
    /** Options */
    Options: {
        long?: boolean,
        separator?: boolean,
        multiline?: boolean,
        type?: string,
        provider?: string,
        providerCanAdd?: boolean, fullLine?: boolean
    };

    /** SubForm Model */
    SubFormModel: FormConfigServerModel;
}
