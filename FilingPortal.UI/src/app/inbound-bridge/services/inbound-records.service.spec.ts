import { TestBed, inject } from '@angular/core/testing';

import { Observable } from 'rxjs/Observable';

import { InboundRecordsService } from './inbound-records.service';
import { InboundRecordsApiService } from './inbound-records-api.service';
import {
  InboundRecordParameterServer, InboundRecordFileModel,
  InboundRecordParameterModel, InboundRecordDocument, InboundRecordDocumentModel
} from '@inbound/models';
import { InboundRecordsValidator } from './inbound-records.validator';
import { FileItem } from 'ng2-file-upload';

class MockInboundRecordsApiService {
  getAdditionalParametersConfiguration(filingHeaderId: any): Observable<InboundRecordParameterServer[]> {
    return new Observable<InboundRecordParameterServer[]>();
  }
  save(model: any) { }
  file(model: any) { }
} class MockInboundRecordsValidator {
  validateDocuments(documents: any[]): boolean { return true; }
  validateAdditionalParameters(parameters: any[]): boolean { return true; }
  validateCommonData(commonData: any[]): boolean { return true; }
}

describe('InboundRecordsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InboundRecordsService,
        { provide: InboundRecordsApiService, useClass: MockInboundRecordsApiService },
        { provide: InboundRecordsValidator, useClass: MockInboundRecordsValidator }
      ]
    });
  });

  it('should be created', inject([InboundRecordsService], (service: InboundRecordsService) => {
    expect(service).toBeTruthy();
  }));

  it('should set filing header Id and inbound record ids', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const filingHeaderId = 3;
    const inboundRecordIds = [1, 67, 23];
    service.setIdentifiersForFiling(filingHeaderId, inboundRecordIds);

    expect(filingHeaderId).toEqual((service as any).filingHeaderId);
    expect(inboundRecordIds).toEqual((service as any).inboundRecordIds);
  }));

  it('should clear filing header Id and inbound record ids', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const filingHeaderId = 3;
    const inboundRecordIds = [1, 67, 23];
    service.setIdentifiersForFiling(filingHeaderId, inboundRecordIds);

    service.clear();

    expect(0).toEqual((service as any).filingHeaderId);
    expect([]).toEqual((service as any).inboundRecordIds);
  }));

  it('should call get additional parameters', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'getAdditionalParametersConfiguration').and.returnValue({ subscribe: () => { } });

    service.getFieldConfigurations();

    expect(apiService.getAdditionalParametersConfiguration).toHaveBeenCalled();
  }));

  it('should call validator', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const validator = TestBed.get(InboundRecordsValidator);
    spyOn(validator, 'validateDocuments').and.returnValue(true);

    service.isDataValid();

    expect(validator.validateDocuments).toHaveBeenCalled();
  }));

  it('should validate sections', inject([InboundRecordsService], (service: InboundRecordsService) => {
    (service as any).validatedSections.documents = false;

    service.isDataValid();

    expect((service as any).validatedSections.documents).toEqual(true);
  }));

  it('should call api to save', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'save');

    service.saveIntermediateResult();

    expect(apiService.save).toHaveBeenCalled();
  }));

  it('should call save api with model including Additional Parameters',
    inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'save');
    (service as any).additionalParametersSource.next([{ id: 1, value: 'test param 1' }, { id: 2, value: 'test param 2' }]);
    const model = new InboundRecordFileModel();
    const param1 = new InboundRecordParameterModel(), param2 = new InboundRecordParameterModel();
    param1.Id = 1;
    param1.Value = 'test param 1';
    param2.Id = 2;
    param2.Value = 'test param 2';
    model.FilingHeaderId = 0;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [param1, param2];

    service.saveIntermediateResult();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call save api with model including Common Data', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'save');
    (service as any).commonDataSource.next([
      { sectionName: 'sec 1', fields: [{ id: 1, value: 'test common data param 1' }, { id: 2, value: 'test common data param 2' }] },
      { sectionName: 'sec 2', fields: [{ id: 3, value: 'test common data param 3' }] },
    ]);
    const model = new InboundRecordFileModel();
    const param1 = new InboundRecordParameterModel(),
      param2 = new InboundRecordParameterModel(),
      param3 = new InboundRecordParameterModel();
    param1.Id = 1;
    param1.Value = 'test common data param 1';
    param2.Id = 2;
    param2.Value = 'test common data param 2';
    param3.Id = 3;
    param3.Value = 'test common data param 3';
    model.FilingHeaderId = 0;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [param1, param2, param3];

    service.saveIntermediateResult();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call save api with model including Filing Header id', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'save');
    (service as any).filingHeaderId = 569;
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = (service as any).filingHeaderId;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [];

    service.saveIntermediateResult();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call save api with model including Inbound Record ids', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'save');
    (service as any).inboundRecordIds = [123, 53, 923];
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = 0;
    model.InboundRecordIds = (service as any).inboundRecordIds;
    model.Documents = [];
    model.Parameters = [];

    service.saveIntermediateResult();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  // TODO: test documents added to model
  /*
    it('should call save api with model including Documents', inject([InboundRecordsService], (service: InboundRecordsService) => {
      let apiService = TestBed.get(InboundRecordsApiService);
      let spy = spyOn(apiService, 'save');
      let documents: InboundRecordDocument[] = [];
      documents[0].fileObj = new FileItem(null, new File(['1'], '1'), new FileUploaderOptions());
      documents[0].type = 'doc type';
      documents[0].description = 'doc descr';
      (service as any).documents = documents;

      let documentModel = new InboundRecordDocumentModel();
      documentModel.Type = 'doc type';
      documentModel.Description = 'doc descr';
      documentModel.Data = null;
      let model = new InboundRecordFileModel();
      model.FilingHeaderId = 0;
      model.InboundRecordIds = [];
      model.Documents = [documentModel];
      model.Parameters = [];

      service.saveIntermediateResult();

      expect(spy).toHaveBeenCalledWith(model);
    }));*/

  it('should call api to file', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'file');

    service.startFiling();

    expect(apiService.file).toHaveBeenCalled();
  }));

  it('should call file api with model including Additional Parameters',
    inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'file');
    (service as any).additionalParametersSource.next([{ id: 1, value: 'test param 1' }, { id: 2, value: 'test param 2' }]);
    const model = new InboundRecordFileModel();
    const param1 = new InboundRecordParameterModel(), param2 = new InboundRecordParameterModel();
    param1.Id = 1;
    param1.Value = 'test param 1';
    param2.Id = 2;
    param2.Value = 'test param 2';
    model.FilingHeaderId = 0;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [param1, param2];

    service.startFiling();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call file api with model including Common Data', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'file');
    (service as any).commonDataSource.next([
      { sectionName: 'sec 1', fields: [{ id: 1, value: 'test common data param 1' }, { id: 2, value: 'test common data param 2' }] },
      { sectionName: 'sec 2', fields: [{ id: 3, value: 'test common data param 3' }] },
    ]);
    const model = new InboundRecordFileModel();
    const param1 = new InboundRecordParameterModel(),
      param2 = new InboundRecordParameterModel(),
      param3 = new InboundRecordParameterModel();
    param1.Id = 1;
    param1.Value = 'test common data param 1';
    param2.Id = 2;
    param2.Value = 'test common data param 2';
    param3.Id = 3;
    param3.Value = 'test common data param 3';
    model.FilingHeaderId = 0;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [param1, param2, param3];

    service.startFiling();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call fill api with model including Filing Header id', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'file');
    (service as any).filingHeaderId = 569;
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = (service as any).filingHeaderId;
    model.InboundRecordIds = [];
    model.Documents = [];
    model.Parameters = [];

    service.startFiling();

    expect(spy).toHaveBeenCalledWith(model);
  }));

  it('should call fill api with model including Inbound Record ids', inject([InboundRecordsService], (service: InboundRecordsService) => {
    const apiService = TestBed.get(InboundRecordsApiService);
    const spy = spyOn(apiService, 'file');
    (service as any).inboundRecordIds = [123, 53, 923];
    const model = new InboundRecordFileModel();
    model.FilingHeaderId = 0;
    model.InboundRecordIds = (service as any).inboundRecordIds;
    model.Documents = [];
    model.Parameters = [];

    service.startFiling();

    expect(spy).toHaveBeenCalledWith(model);
  }));
});
