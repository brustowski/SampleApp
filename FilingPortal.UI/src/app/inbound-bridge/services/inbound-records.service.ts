import { Injectable } from '@angular/core';

import { FileUploader } from 'ng2-file-upload';

import { InboundRecordsApiService } from './inbound-records-api.service';
import { InboundRecordsValidator } from './inbound-records.validator';

import {
  InboundRecordParameter,
  InboundRecordParameterModel,
  InboundRecordFileModel,
  InboundRecordDocumentModel,
  InboundRecordCommonData,
  InboundRecordCommonDataServer,
  InboundRecordCommonDataBuilder,
  InboundRecordDocumentServer,
  InboundRecordDocumentBuilder,
  InboundSectionsValidation,
  InboundRecordDocument,
  InboundRecordConfigurationServer,
  FieldErrors
} from '@inbound/models';

import { FieldOptionsBuilder } from '@common/fields/models/FieldOptions';
import { FieldBuilder } from '@common/fields/models';
import { Manifest } from '@common/fields/models/manifest';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import 'rxjs/add/operator/map';
import { convertParameter } from '@inbound/mappings';

@Injectable()
export class InboundRecordsService {
  public uploader: FileUploader;
  private filingHeaderId: number = 0;
  private inboundRecordIds: any[] = [];
  private validatedSections: InboundSectionsValidation = { documents: false };

  public documentsSource = new BehaviorSubject<InboundRecordDocument[]>([]);
  public documents = this.documentsSource.asObservable();

  private additionalParametersSource = new BehaviorSubject<
    InboundRecordParameter[]
  >([]);
  public additionalParameters = this.additionalParametersSource.asObservable();

  private commonDataSource = new BehaviorSubject<InboundRecordCommonData[]>([]);
  public commonData = this.commonDataSource.asObservable();

  constructor(
    private _apiService: InboundRecordsApiService,
    private _validator: InboundRecordsValidator
  ) {
    this.uploader = new FileUploader({
      url: '',
      method: 'POST'
    });
  }

  clear() {
    this.validatedSections = { documents: false };
    this.filingHeaderId = 0;
    this.inboundRecordIds = [];
    this.commonDataSource.next([]);
    this.documentsSource.next([]);

    this.additionalParametersSource.next([]);
    this.uploader.clearQueue();
  }

  setIdentifiersForFiling(
    filingHeaderId: number,
    inboundRecordIds: any[]
  ): void {
    this.filingHeaderId = filingHeaderId;
    this.inboundRecordIds = inboundRecordIds;
  }

  getFilingHeaderId(): number {
    return this.filingHeaderId;
  }

  getRecordIds(): any[] {
    return this.inboundRecordIds;
  }

  getFieldConfigurations(): Observable<InboundRecordConfigurationServer> {
    const subj = new Subject<InboundRecordConfigurationServer>();

    this._apiService
      .getAdditionalParametersConfiguration(this.filingHeaderId)
      .subscribe(config => {
        // additional parameters
        const params: InboundRecordParameter[] = [];
        config.AdditionalParameters.forEach(param =>
          params.push(convertParameter(param))
        );
        this.additionalParametersSource.next(params);
        // common data
        const commonData: InboundRecordCommonData[] = [];
        config.CommonData.forEach(cd =>
          commonData.push(this.convertSection(cd))
        );
        this.commonDataSource.next(commonData);
        // documents
        const documents: InboundRecordDocument[] = [];
        config.Documents.forEach(d => documents.push(this.convertDocument(d)));
        this.documentsSource.next(documents);

        subj.next(config);
      });

    return subj.asObservable();
  }

  getDocuments(): Observable<InboundRecordDocument[]> {
    return this.documents;
  }

  getValidatedSections() {
    return this.validatedSections;
  }

  saveIntermediateResult() {
    const model = this.createInboundRecordFileModel();
    return this._apiService.save({ Models: [model] });
  }

  startFiling() {
    const model = this.createInboundRecordFileModel();
    return this._apiService.file({ Models: [model] });
  }

  validate(): FieldErrors[] {
    const additionalParameters = this.additionalParametersSource.getValue();
    const allParams = this.commonDataSource
      .getValue()
      .reduce((arr, x) => arr.concat(x.fields), additionalParameters);
    return this._validator.validateFields(allParams);
  }

  isDataValid(): boolean {
    this.validatedSections.documents = true;
    const documents = this.documentsSource.getValue();

    const additionalParameters = this.additionalParametersSource.getValue();
    additionalParameters.forEach(p => (p.options.validationOn = true));
    this.additionalParametersSource.next(additionalParameters);

    const commonData = this.commonDataSource.getValue();
    commonData.forEach(d =>
      d.fields.forEach(f => (f.options.validationOn = true))
    );
    this.commonDataSource.next(commonData);

    return (
      this._validator.isValidDocuments(documents) &&
      this._validator.isValidAdditionalParameters(additionalParameters) &&
      this._validator.isValidCommonData(commonData)
    );
  }

  validateFiles(): boolean {
    this.validatedSections.documents = true;
    const documents = this.documentsSource.getValue();
    return this._validator.isValidDocuments(documents);
  }

  getManifest(recordId: number): Observable<Manifest> {
    const builder = new FieldBuilder();
    return this._apiService.getManifest(recordId).map(manifest => {
      const man = new Manifest();
      man.rawManifest = manifest.ManifestText;
      man.fields = manifest.Fields.map(field =>
        builder.create()
          .name(field.Name)
          .title(field.Title)
          .value(field.Value)
          .options(new FieldOptionsBuilder()
            .create()
            .customOption('long', field.Options && field.Options.long)
            .customOption('separator', field.Options && field.Options.separator)
            .customOption('multiline', field.Options && field.Options.multiline)
            .build())
          .build());
      return man;
    }
    );
  }

  private convertSection(param: InboundRecordCommonDataServer) {
    return new InboundRecordCommonDataBuilder()
      .create()
      .sectionName(param.SectionName)
      .fields(
        param.Fields.map<InboundRecordParameter>(field =>
          convertParameter(field)
        )
      )
      .build();
  }

  private convertDocument(
    document: InboundRecordDocumentServer
  ): InboundRecordDocument {
    return new InboundRecordDocumentBuilder()
      .create()
      .id(document.Id)
      .filingHeaderId(document.FilingHeaderId)
      .name(document.Name)
      .type(document.Type)
      .description(document.Description)
      .status(document.Status)
      .isManifest(document.IsManifest)
      .build();
  }

  private convertInboundRecordParameter(param: InboundRecordParameter): InboundRecordParameterModel[] {
    const results: InboundRecordParameterModel[] = [];
    if (param.id) {
      const model = new InboundRecordParameterModel();
      model.Id = param.id;
      model.Value = param.value;
      results.push(model);
    }
    if (param.additionalFields && param.additionalFields.length) {
      param.additionalFields.forEach(x => results.push(...this.convertInboundRecordParameter(x)));
    }
    return results;
  }

  public createInboundRecordFileModel(): InboundRecordFileModel {
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = this.filingHeaderId;
    model.Parameters = this.getAdditionalParameterModels().concat(
      this.getCommonDataParameterModels()
    );
    model.Documents = this.getDocumentModels();
    return model;
  }

  private getAdditionalParameterModels() {
    const params: InboundRecordParameterModel[] = [];
    this.additionalParametersSource
      .getValue()
      .forEach(param => params.push(...this.convertInboundRecordParameter(param)));
    return params;
  }

  private getCommonDataParameterModels() {
    const params: InboundRecordParameterModel[] = [];
    this.commonDataSource
      .getValue()
      .forEach(section =>
        section.fields.forEach(param =>
          params.push(...this.convertInboundRecordParameter(param))
        )
      );
    return params;
  }

  private getDocumentModels(): InboundRecordDocumentModel[] {
    return this.documentsSource.getValue().map(
      d =>
        <InboundRecordDocumentModel>{
          Id: d.id,
          Name: d.name,
          Type: d.type,
          Description: d.description,
          Data: d.fileObj ? d.fileObj._file : null,
          Status: d.status,
          IsManifest: d.isManifest
        }
    );
  }

  setModel(config: InboundRecordConfigurationServer): void {
    // additional parameters
    const params: InboundRecordParameter[] = [];
    config.AdditionalParameters.forEach(param =>
      params.push(convertParameter(param))
    );
    this.additionalParametersSource.next(params);
    // common data
    const commonData: InboundRecordCommonData[] = [];
    config.CommonData.forEach(cd => commonData.push(this.convertSection(cd)));
    this.commonDataSource.next(commonData);
    // documents
    const documents: InboundRecordDocument[] = [];
    config.Documents.forEach(d => documents.push(this.convertDocument(d)));
    this.documentsSource.next(documents);
  }
}
