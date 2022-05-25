import { BaseInboundRecordField } from '@common/models';

import { LookupFieldOptionsBuilder, FieldType, LookupFieldSource, SourceType } from '@common/fields/models';

import { isSimpleField, isComplexField, isDropdownField, isAddressField } from '@common/typeguards/inbound-record-field';
import { InboundRecordParameterBuilder, InboundRecordParameter } from '@inbound/models/inbound-record-parameter';

export function convertParameter(param: BaseInboundRecordField): InboundRecordParameter {
  const optionsBuilder = new LookupFieldOptionsBuilder()
    .create()
    .enableValidation();

  if (isDropdownField(param)) {
    optionsBuilder
      .source(<LookupFieldSource>{
        name: param.ProviderName,
        canAdd: false,
        type: param.IsDynamicProvider ? SourceType.Handbook : SourceType.Form
      })
      .fieldType(FieldType.Lookup)
      .searchable(false);
  }

  if (isAddressField(param)) {
    optionsBuilder
      .source(<LookupFieldSource>{
        name: param.ProviderName,
        canAdd: false,
        type: param.IsDynamicProvider ? SourceType.Handbook : SourceType.Form
      })
      .fieldType(FieldType.Lookup)
      .searchable(false);
  }

  if (isSimpleField(param)) {
    optionsBuilder.maxLength(param.MaxLength)
      .required(param.IsMandatory)
      .disable(param.IsDisabled)
      .prefix(param.Prefix)
      .dependsOn(param.DependOn)
      .class(param.Class)
      ;
  }
  const paramBuilder = new InboundRecordParameterBuilder()
    .create()
    .title(param.Title)
    .type(param.Type)
    .markedForFiltering(param.MarkedForFiltering)
    .options(optionsBuilder.build());

  if (isSimpleField(param)) {
    paramBuilder
      .id(param.Id)
      .recordId(param.RecordId)
      .parentRecordId(param.ParentRecordId)
      .defValue(param.DefaultValue)
      .confirmationNeeded(param.ConfirmationNeeded);

  } else {
    if (isComplexField(param)) {
      paramBuilder.additionalFields(param.Fields ? param.Fields.map(x => convertParameter(x)) : null);
    }
  }

  return paramBuilder.build();
}
