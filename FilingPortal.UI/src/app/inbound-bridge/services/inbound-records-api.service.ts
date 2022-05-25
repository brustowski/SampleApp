import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { convertModelToFormData } from '@app/functions';

import { InboundConfigurationService } from './inbound-configuration.service';

import {
  InboundRecordValidationResultViewModel,
  InboundRecordConfigurationServer,
  InboundRecordFileModel,
  InboundRecordDocument,
  ImportRecordRowServer,
  RecordFieldToRowConverter,
  InboundRecordListActions,
  FormConfigServerModel,
  FilingConfiguration,
  FilingConfigurationField,
  FilingConfigurationSection
} from '@inbound/models';
import {
  FilingResultBuilder,
  InboundRecordField,
  FilingConfigurationServer,
  FilingHeaderConfirmation,
  ImporterChangeModel,
} from '@common/models';
import { FilterParsedConfig } from '@common/filters/models/Filter';
import { ManifestServerModel } from '@inbound/models/manifest-server-model';
import { HttpClient } from '@angular/common/http';
import { HttpService } from '@common/services';
import { ValidationResultWithFieldsErrorsViewModelTyped } from '@common/models/models';
import { of } from 'rxjs';

@Injectable()
export class InboundRecordsApiService {

  constructor(
    private http: HttpClient,
    private httpService: HttpService,
    private mappingsService: InboundConfigurationService
  ) { }

  deleteRecords(recordIds: (string | number)[]): Observable<any> {
    return this.mappingsService.getApiPath().switchMap(path => this.http.post(`${path}/delete`, recordIds));
  }

  restoreRecords(recordIds: (string | number)[]): Observable<any> {
    return this.mappingsService.getApiPath().switchMap(path => this.http.post(`${path}/restore`, recordIds));
  }

  validateRecords(ids: number[]): Observable<InboundRecordValidationResultViewModel> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<InboundRecordValidationResultViewModel>(`${path}/validate-selected-records`, ids)
      );
  }

  getAvailableActions(ids: number[]): Observable<InboundRecordListActions> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<InboundRecordListActions>(`${path}/available-actions`, ids));
  }

  validateFilteredRecords(filterConfiguration: FilterParsedConfig): Observable<InboundRecordValidationResultViewModel> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<InboundRecordValidationResultViewModel>(`${path}/validate-filtered-records`, filterConfiguration)
      );
  }

  getRecordIds(filingHeaderIds: number[]): Observable<number[]> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<number[]>(`${path}/filing/record-ids`, filingHeaderIds));
  }

  startUnitTrainFiling(ids: number[]): Observable<number> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<number>(`${path}/filing/start-unit-train`, ids));
  }

  startFiling(ids: (string | number)[]): Observable<number[]> {
    return this.mappingsService.getApiPath().switchMap(path => this.http.post<number[]>(`${path}/filing/start`, ids));
  }

  validate(filterConfiguration: FilterParsedConfig): Observable<void> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<void>(`${path}/validate/filtered`, filterConfiguration));
  }

  startFilteredFiling(filterConfiguration: FilterParsedConfig): Observable<number[]> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<number[]>(`${path}/filing/start-filtered`, filterConfiguration));
  }

  startUnitTrainFilteredFiling(filterConfiguration: FilterParsedConfig): Observable<number> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<number>(`${path}/filing/start-unit-train-filtered`, filterConfiguration));
  }

  getAdditionalParametersConfiguration(filingHeaderId: number): Observable<InboundRecordConfigurationServer> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<InboundRecordConfigurationServer>(`${path}/field-config/${filingHeaderId}`, null)
      );
  }

  getSingleFilingParams(filingHeaderIds: number[]): Observable<ImportRecordRowServer[]> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<{ AdditionalParameters: InboundRecordField[] }>(
          `${path}/field-config/single-filing`,
          filingHeaderIds
        )
      )
      .map(res => {
        const converter = new RecordFieldToRowConverter();

        res.AdditionalParameters.forEach(x => converter.addValue(x));
        return converter.convert();
      });
  }

  getFilingConfiguration(filingHeaderId: number): Observable<FilingConfiguration> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.get<FilingConfigurationServer>(`${path}/filing/configuration/${filingHeaderId}`))
      .map<FilingConfigurationServer, FilingConfiguration>(x => this.ConfigurationClientModel(x));
  }

  private ConfigurationClientModel(configuration: FilingConfigurationServer): FilingConfiguration {
    const result = new FilingConfiguration();
    result.filingHeaderId = configuration.FilingHeaderId;

    result.fields = (configuration.Fields || []).map(
      f =>
        <FilingConfigurationField>{
          filingHeaderId: f.FilingHeaderId,
          id: f.Id,
          title: f.Title,
          name: f.Name,
          type: f.Type,
          value: f.Value,
          isDisabled: f.IsDisabled,
          description: f.Description,
          isMandatory: f.IsMandatory,
          isVisibleOn7501: f.IsVisibleOn7501,
          isVisibleOnRuleDrivenData: f.IsVisibleOnRuleDrivenData,
          maxLength: f.MaxLength,
          order: f.Order,
          parentRecordId: f.ParentRecordId,
          recordId: f.RecordId,
          sectionName: f.SectionName,
          sectionTitle: f.SectionTitle,
          Field: f.Field
        }
    );

    result.documents = (configuration.Documents || []).map(
      d =>
        <InboundRecordDocument>{
          id: d.Id,
          description: d.Description,
          isManifest: d.IsManifest,
          name: d.Name,
          filingHeaderId: configuration.FilingHeaderId,
          status: d.Status,
          type: d.Type
        }
    );
    result.sections = (configuration.Sections || []).map(
      s =>
        <FilingConfigurationSection>{
          id: s.Id,
          name: s.Name,
          title: s.Title,
          parentId: s.ParentId,
          isSingleSection: s.IsSingleSection,
          displayAsGrid: s.DisplayAsGrid
        }
    );

    return result;
  }

  addConfiguration(filingHeaderId: number, nodeName: string, parentId: any): Observable<FilingConfiguration> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.post<FilingConfigurationServer>(`${path}/filing/configuration/${filingHeaderId}/${nodeName}/${parentId}`, {})
      )
      .map<FilingConfigurationServer, FilingConfiguration>(x => this.ConfigurationClientModel(x));
  }

  getConfiguration(filingHeaderId: number, nodeName: string, id: any) {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.get<FilingConfigurationServer>(`${path}/filing/configuration/${filingHeaderId}/${nodeName}/${id}`, {})
      )
      .map<FilingConfigurationServer, FilingConfiguration>(x => this.ConfigurationClientModel(x));
  }

  deleteConfiguration(filingHeaderId: number, nodeName: string, parentId: any): Observable<boolean> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.delete<boolean>(`${path}/filing/configuration/${filingHeaderId}/${nodeName}/${parentId}`)
      );
  }

  getManifest(recordId: number): Observable<ManifestServerModel> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.get<ManifestServerModel>(`${path}/manifest/${recordId}`));
  }

  save(model: { Models: InboundRecordFileModel[] }): Observable<FilingResultBuilder> {
    return this.mappingsService.getMvcPath().switchMap(path => {
      const url = `${path}/save`;
      const sendable = convertModelToFormData(model);
      return this.httpService.requestRaw(url, sendable);
    });
  }

  file(model: { Models: InboundRecordFileModel[] }): Observable<FilingResultBuilder> {
    return this.mappingsService.getMvcPath().switchMap(path => {
      const url = `${path}/file`;
      const sendable = convertModelToFormData(model);
      return this.httpService.requestRaw(url, sendable);
    });
  }

  canBeEdited(filingHeaderId: number[]): Observable<boolean> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<boolean>(`${path}/filing/validate`, filingHeaderId));
  }

  undo(filingHeaderIds: number[]): Observable<any> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post(`${path}/filing/cancel`, filingHeaderIds));
  }

  getAddConfig(): Observable<FormConfigServerModel> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.get<FormConfigServerModel>(`${path}/get-add-form-config`));
  }

  public getRecord<T>(id: number): Observable<T> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path =>
        this.http.get<T>(`${path}/${id}`)
      );
  }

  saveRecord<T extends { Id: number }>(model: T): Observable<ValidationResultWithFieldsErrorsViewModelTyped<number>> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<ValidationResultWithFieldsErrorsViewModelTyped<number>>(`${path}/save-inbound`, model));
  }

  recalculateFieldValues(model: InboundRecordFileModel): Observable<InboundRecordFileModel> {
    return this.mappingsService.getApiPath().switchMap(() => {
      const url = this.mappingsService.getFieldRecalculationPath();
      if (url) {
        const body = { FilingHeaderId: model.FilingHeaderId, Parameters: model.Parameters };
        return this.http.post<InboundRecordFileModel>(url, body);
      }
      return of(null);
    });
  }

  updateConfirmationStatus(confirmations: FilingHeaderConfirmation[]): Observable<FilingHeaderConfirmation[]> {
    const url = this.mappingsService.getFilingHeaderConfirmationPath();
    if (url) {
      return this.http.post<FilingHeaderConfirmation[]>(url, confirmations);
    }
    return of([]);
  }

  getJobNumber(recordIds: (number | string)[]): Observable<number> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post<number>(`${path}/job-numbers`, recordIds));
  }

  setImporter(id: string | number, value: string): Observable<any> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post(`${path}/set-importer/${id}`, <ImporterChangeModel>{ ClientId: value }));
  }
  
  setFtzoperator(id: string | number, value: string): Observable<any> {
    return this.mappingsService
      .getApiPath()
      .switchMap(path => this.http.post(`${path}/set-ftzoperator/${id}`, <ImporterChangeModel>{ ClientId: value }));
  }
}
