import { TestBed, inject } from '@angular/core/testing';
import { Observable } from 'rxjs/Observable';

import { FileItem, FileUploader } from 'ng2-file-upload';

import { NotificationService } from '@common/notification/notification.service';

import { InboundRecordsApiService } from './inbound-records-api.service';
import { InboundRecordsValidator } from './inbound-records.validator';

import { InboundRecordValidationResultViewModel, InboundRecordDocument } from '@inbound/models';

describe('InboundRecordsValidator', () => {
  beforeEach(() => {
    const fakeApiService: any = {
      validateRecords: (ids: any[]) => {
        return Observable.of(new InboundRecordValidationResultViewModel());
      }
    };
    fakeApiService.validateRecords = jasmine.createSpy('validateRecords', fakeApiService.validateRecords)
      .and.returnValue({ subscribe: () => { } });
    const fakeNotificationService: any = { alert: (notification) => { } };
    fakeNotificationService.alert = jasmine.createSpy('alert', fakeNotificationService.alert);


    TestBed.configureTestingModule({
      providers: [
        InboundRecordsValidator,
        { provide: InboundRecordsApiService, useValue: fakeApiService },
        { provide: NotificationService, useValue: fakeNotificationService }
      ]
    });
  });

  it('should be created', inject([InboundRecordsValidator], (service: InboundRecordsValidator) => {
    expect(service).toBeTruthy();
  }));

  it('should return true if documents are valid', inject([InboundRecordsValidator], (service: InboundRecordsValidator) => {
    const uploader: FileUploader = new FileUploader({ url: 'some' });

    const docs: InboundRecordDocument[] = [];
    const doc1: InboundRecordDocument = new InboundRecordDocument();
    doc1.fileObj = new FileItem(uploader, new File(['1212'], 'aa'), {});
    doc1.type = 'some type';
    const doc2: InboundRecordDocument = new InboundRecordDocument();
    doc2.fileObj = new FileItem(uploader, new File(['1212'], 'aa'), {});
    doc2.type = 'another type';

    docs.push(doc1);
    docs.push(doc2);

    expect(service.isValidDocuments(docs)).toEqual(true);
  }));

  it('should return true if documents are not valid', inject([InboundRecordsValidator], (service: InboundRecordsValidator) => {

    const docs: InboundRecordDocument[] = [];
    const doc1: InboundRecordDocument = new InboundRecordDocument();
    doc1.type = 'some type';
    const doc2: InboundRecordDocument = new InboundRecordDocument();
    doc2.type = null;

    docs.push(doc1);
    docs.push(doc2);

    expect(service.isValidDocuments(docs)).toEqual(false);
  }));

  it('should call validation on backend for records', inject([InboundRecordsValidator], (service: InboundRecordsValidator) => {
    const apiService = TestBed.get(InboundRecordsApiService);

    service.validateSelectedRecords([23, 56]);

    expect(apiService.validateRecords).toHaveBeenCalled();
  }));
});
