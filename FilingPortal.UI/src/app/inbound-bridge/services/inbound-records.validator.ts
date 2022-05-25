import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Observable';

import * as R from 'ramda';

import { InboundRecordsApiService } from './inbound-records-api.service';

import {
  InboundRecordValidationResultViewModel,
  InboundRecordDocument,
  InboundRecordParameter,
  InboundRecordCommonData,
  InboundRecordDocumentStatus,
  FieldErrors
} from '@inbound/models';
import { FilterParsedConfig } from '@common/filters/models/Filter';
import {
  ImportRecordParameterValidator,
  ImportRecordParameterMandatoryValidator,
  ImportRecordParameterFloatingPointValidator,
  ImportRecordParameterDateValidator,
  ImportRecordParameterConfirmationValidator
} from '../validators';
import { Field } from '@common/fields/models';

@Injectable()
export class InboundRecordsValidator {
  private validators: ImportRecordParameterValidator[] = [];

  constructor(private inboundRecordsApiService: InboundRecordsApiService) {
    this.initValidators();
  }

  /**
   * Initialize Import Record Parameter validators
   */
  private initValidators(): void {
    this.validators.push(new ImportRecordParameterMandatoryValidator());
    this.validators.push(new ImportRecordParameterFloatingPointValidator());
    this.validators.push(new ImportRecordParameterDateValidator());
    this.validators.push(new ImportRecordParameterConfirmationValidator());
  }

  public isValidDocuments(documents: InboundRecordDocument[]): boolean {
    return documents.every(d => {
      return this.validateDocument(d);
    });
  }

  public isValidAdditionalParameters(parameters: InboundRecordParameter[]) {
    return R.all(p => p.options.errors.length === 0, parameters);
  }

  public isValidCommonData(commonData: InboundRecordCommonData[]) {
    return R.all(
      p => R.all(f => f.options.errors.length === 0, p.fields),
      commonData
    );
  }

  public validateDocument(doc: InboundRecordDocument): boolean {
    let result = true;
    if (doc.status !== InboundRecordDocumentStatus.Deleted) {
      result =
        (doc.status !== InboundRecordDocumentStatus.New ||
          doc.fileObj != null) &&
        this.validateDocumentType(doc.type);
    }
    return result;
  }

  public validateDocumentType(docType: string): boolean {
    return docType != null && docType !== '';
  }

  /**
   * Validate documents for errors
   * @param documents InboundRecordDocuments Array
   */
  public validateDocuments(documents: InboundRecordDocument[]): FieldErrors[] {
    const errors: FieldErrors[] = [];
    documents.forEach(x => {
      const error: FieldErrors = { FieldId: x.id, Errors: [] };
        if (!this.validateDocument(x)) {
          error.Errors.push('Document type is mandatory');
        }
      errors.push(error);
    });
    return errors;
  }

  /**
   * Validate paramters for errors
   * @param parameters InboundRecordParameters Array
   */
  public validateFields(parameters: InboundRecordParameter[]): FieldErrors[] {
    const errors: FieldErrors[] = [];
    parameters.forEach(x => {
      const error: FieldErrors = { FieldId: x.id || x.additionalFields[0].id, Errors: [] };
      this.validators.forEach(validator => {
        const result = validator.validate(x);
        if (result) {
          error.Errors.push(result);
        }
      });
      errors.push(error);
    });
    return errors;
  }

  public validateSelectedRecords(
    ids: number[]
  ): Observable<InboundRecordValidationResultViewModel> {
    return this.inboundRecordsApiService.validateRecords(ids);
  }

  public validateFilteredRecords(
    filterConfiguration: FilterParsedConfig
  ): Observable<InboundRecordValidationResultViewModel> {
    return this.inboundRecordsApiService.validateFilteredRecords(
      filterConfiguration
    );
  }
}
