import { Field } from '.';

/**
 * Manifest class
 */
export class Manifest {
    /** Raw manifest text */
    rawManifest: string;
    /** Manifest parsed fields */
    fields: Field[];
}
