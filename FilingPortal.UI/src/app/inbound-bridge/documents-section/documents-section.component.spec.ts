import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FileUploadModule } from 'ng2-file-upload';
import { FormsModule } from '@angular/forms';
import { FieldsModule } from '../../../common/fields/fields.module';

import { DocumentsSectionComponent } from './documents-section.component';
import { InboundRecordsService } from '../services/inbound-records.service';
import { InboundRecordsValidator } from '../services/inbound-records.validator';
import { Observable } from 'rxjs';
import { FieldsApiService } from '../../../common/fields/services/fields-api.service';
import { ModalService } from '../../../common/services/modal.service';
import { InboundRecordDocument } from '../models/InboundRecordDocument';

describe('DocumentsSectionComponent', () => {
  let component: DocumentsSectionComponent;
  let fixture: ComponentFixture<DocumentsSectionComponent>;
  let irService: InboundRecordsService;
  let validator: InboundRecordsValidator;
  let fieldsApi: FieldsApiService;
  let modal: ModalService;

  beforeEach(async(() => {

    const fakeInboundRecordsService: any = { getDocuments: () => [] };
    const fakeInboundRecordsValidator: any = { validateDocumentType: () => true };
    const fakeFieldsApiService: any = { getSelectFieldOptions: (searchSettings: any) => new Observable<any>() };
    const fakeModalService: any = { confirm: () => Promise.resolve(true) };

    fakeInboundRecordsService.getDocuments = jasmine.createSpy('getDocuments', fakeInboundRecordsService.getDocuments);
    fakeInboundRecordsValidator.validateDocumentType
      = jasmine.createSpy('validateDocumentType', fakeInboundRecordsValidator.validateDocumentType);
    fakeFieldsApiService.getSelectFieldOptions = jasmine.createSpy('getSelectFieldOptions', fakeFieldsApiService.getSelectFieldOptions);
    fakeModalService.confirm = jasmine.createSpy('confirm', fakeFieldsApiService.confirm);

    TestBed.configureTestingModule({
      declarations: [DocumentsSectionComponent],
      providers: [
        { provide: InboundRecordsService, useValue: fakeInboundRecordsService },
        { provide: InboundRecordsValidator, useValue: fakeInboundRecordsValidator },
        { provide: FieldsApiService, useValue: fakeFieldsApiService },
        { provide: ModalService, useValue: fakeModalService },
      ],
      imports: [FileUploadModule, FormsModule, FieldsModule]
    })
      .compileComponents();

    irService = fakeInboundRecordsService as InboundRecordsService;
    validator = fakeInboundRecordsValidator as InboundRecordsValidator;
    fieldsApi = fakeFieldsApiService as FieldsApiService;
    modal = fakeModalService as ModalService;

    fixture = TestBed.createComponent(DocumentsSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should ask confirmation before document deletion', () => {
    component.documents = [];
    component.documents.push(new InboundRecordDocument());

    component.removeDocument(new InboundRecordDocument());

    expect(modal.confirm).toHaveBeenCalled();
  });
});
