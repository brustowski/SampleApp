import {
  BaseInboundRecordField,
  InboundRecordField,
  ComplexInboundRecordField,
  DropdownInboundRecordField,
  AddressInboundRecordField
} from '@common/models';

export function isSimpleField(field: BaseInboundRecordField): field is InboundRecordField {
  return field.Type !== 'Complex';
}

export function isComplexField(field: BaseInboundRecordField): field is ComplexInboundRecordField {
  return field.Type === 'Complex';
}

export function isDropdownField(field: BaseInboundRecordField): field is DropdownInboundRecordField {
  return field.Type === 'Lookup';
}

export function isAddressField(field: BaseInboundRecordField): field is AddressInboundRecordField {
  return field.Type === 'Address';
}
