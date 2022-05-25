import { Injectable } from '@angular/core';
import {
  FormConfig, LookupFieldOptionsBuilder, LookupFieldSource, SourceType,
  FieldOptionsBuilder, FieldType
} from '@common/fields/models';
import { Observable } from 'rxjs';
import { InboundRecordParameterBuilder, FieldServerModel } from '@inbound/models';
import { ValidationResultWithFieldsErrorsViewModelTyped } from '@common/models';
import { InboundRecordsApiService } from './inbound-records-api.service';

@Injectable({
  providedIn: 'root'
})
export class AddInboundRecordService {
  constructor(private _apiService: InboundRecordsApiService) { }

  getAddFormConfig(): Observable<FormConfig> {
    return this._apiService.getAddConfig().map(config => this.convertToFormConfig(config.Fields));
  }

  convertToFormConfig(models: FieldServerModel[]): FormConfig {
    const builder = new InboundRecordParameterBuilder();
    const man = new FormConfig();
    man.fields = models.map(field => {
      builder
        .create()
        .title(field.Title)
        .name(field.Name)
        .defValue(field.Value)
        .type(field.Options.type);

      if (field.SubFormModel) {
        const subFormConfig = this.convertToFormConfig(field.SubFormModel.Fields);
        builder.additionalFields(subFormConfig.fields);
      }

      const optionsBuilder =
        field.Options.type === 'Lookup' || field.Options.type === 'Address'
          ? new LookupFieldOptionsBuilder()
            .create()
            .source(
              new LookupFieldSource(field.Options.provider, SourceType.Form, field.Options.providerCanAdd || false)
            )
          : new FieldOptionsBuilder().create();
      optionsBuilder
        .name(field.Name)
        .enableValidation()
        .fieldType(FieldType[field.Options.type])
        .customOption('long', field.Options && field.Options.long)
        .customOption('separator', field.Options && field.Options.separator)
        .customOption('multiline', field.Options && field.Options.multiline)
        .customOption('fullLine', field.Options && field.Options.fullLine)
        .dependsOn(field.Options['dependsOn'])
        .dependsOnValues(field.Options['dependsOnValues']);

      builder.options(optionsBuilder.build());

      return builder.build();
    });
    return man;
  }

  public GetRecord<T>(id: number): Observable<T> {
    return this._apiService.getRecord<T>(id);
  }

  addOrEditRecord<T extends { Id: number }>(model: T): Observable<ValidationResultWithFieldsErrorsViewModelTyped<number>> {
    return this._apiService.saveRecord(model);
  }

  setValidationErrors(
    config: FormConfig,
    validationModel: ValidationResultWithFieldsErrorsViewModelTyped<number>
  ): any {
    const regex = /\[(\d)*\]/i;

    config.fields.forEach(x => (x.options.errors = []));
    validationModel.FieldsErrors.forEach(x => {
      const field = config.fields.find(f => f.name === x.FieldName || f.name === x.FieldName.replace(regex, '').trim());
      if (field) {
        field.options.errors.push(x.Message);
      }
    });
  }
}
