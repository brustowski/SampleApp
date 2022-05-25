import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentsTableSectionComponent } from './documents-table-section.component';
import { InboundRecordsService } from '../services/inbound-records.service';
import { InboundRecordsApiService } from '../services/inbound-records-api.service';
import { BehaviorSubject } from 'rxjs';
import { InboundRecordDocument } from '../models/InboundRecordDocument';

class MockInboundRecordsService {
  public documentsSource = new BehaviorSubject<InboundRecordDocument[]>([]);
  public documents = this.documentsSource.asObservable();
}
class MockInboundRecordsApiService { }

describe('DocumentsTableSectionComponent', () => {
  let component: DocumentsTableSectionComponent;
  let fixture: ComponentFixture<DocumentsTableSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DocumentsTableSectionComponent]
      , providers: [
        { provide: InboundRecordsService, useClass: MockInboundRecordsService }
        , { provide: InboundRecordsApiService, useClass: MockInboundRecordsApiService }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentsTableSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });

  it('should contains one table row for each docuemnt', () => {
    component.documents = [
      <InboundRecordDocument>{ name: 'first', type: 'DOC1', description: 'first description' }
      , <InboundRecordDocument>{ name: 'second', type: 'DOC3', description: 'second description' }
      , <InboundRecordDocument>{ name: 'third', type: 'DOC2', description: 'third description' }
    ];
    fixture.detectChanges();
    const elem: HTMLElement = fixture.nativeElement;
    expect(elem.querySelectorAll('tbody > tr').length).toBe(3);
  });
});
