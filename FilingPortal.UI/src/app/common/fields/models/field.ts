import { FieldOptions } from './FieldOptions';
import { LookupFieldOptions } from '.';

/**
 * Field base class
 */
export class Field {
    /** Field name */
    name: string;
    /** Field title */
    title: string;
    /** Field value */
    value: string;
    /** Field type */
    type: string;
    /** Field options*/
    options: FieldOptions | LookupFieldOptions = new FieldOptions();
}
