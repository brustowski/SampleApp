import { FieldOptions, LookupFieldOptions, FieldType } from '@common/fields/models';

export function isLookup(options: FieldOptions): options is LookupFieldOptions {
    return options.type === FieldType.Lookup;
}
