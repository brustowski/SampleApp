import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { Component, Input, ChangeDetectorRef } from '@angular/core';

import { PageParams } from '@common/grid/models';

import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { PipelineListComponent } from './pipeline-list.component';
import {
  GridService,
  GridPageApiService,
  GridPageColumnsService,
  GridStorageService
} from '@common/grid/services';
import { EventsService, ModalService } from '@common/services';
import {
  InboundRecordsValidator,
  InboundRecordsService,
  InboundRecordsApiService
} from '@inbound/services';

@Component({ selector: 'ngx-datatable', template: '' })
class NgxDatatableStubComponent {
  @Input() headerHeight: any;
  @Input() footerHeight: any;
  @Input() rowHeight: any;
  @Input() rows: any;
  @Input() columns: any;
  @Input() columnMode: any;
  @Input() reorderable: any;
  @Input() externalPaging: any;
  @Input() externalSorting: any;
  @Input() scrollbarH: any;
  @Input() selected: any;
  @Input() selectionType: any;
  @Input() selectAllRowsOnPage: any;
  @Input() count: any;
  @Input() offset: any;
  @Input() limit: any;
  @Input() sorts: any;
  @Input() rowIdentity: any;
  @Input() displayCheck: any;
  @Input() rowClass: any;
}

@Component({ selector: 'lxft-grid-toolbar', template: '' })
class GridToolbarStubComponent {
  @Input() pageOptions: any;
  @Input() isLoading: boolean;
}

@Component({ selector: 'lxft-grid-count-results', template: '' })
class GridCountResultsStubComponent {
  @Input() pageOptions: any;
}
@Component({ selector: 'lxft-grid-selection-toolbar', template: '' })
class GridSelectionToolbarStubComponent {
  @Input() selectedRows: any[];
  onClear() {}
}

@Component({ selector: 'lxft-filters-panel', template: '' })
class FiltersPanelStubComponent {
  @Input() gridName: string;
  onApply(e) {}
  onReset() {}
}

@Component({ selector: 'app-colored-label', template: '' })
class ColoredLabelStubComponent {
  @Input() title: string;
  @Input() css: string;
}
@Component({ selector: 'app-review-and-file', template: '' })
class ReviewAndFileInboundRecordsStubComponent {}
@Component({ selector: 'lxft-icon-tooltip', template: '' })
class IconTooltipStubComponent {
  @Input() messages: any;
  @Input() iconType: any;
}

class MockGridService {
  getPageParams(
    paginationOptions: any,
    sortOptions: any,
    filtersOptions: any[]
  ) {
    return <PageParams>{};
  }
}
class MockEventsService {}
class MockGridStorageService {}
class MockGridPageApiService {
  getGridColumnsConfig(gridName: any): Observable<any> {
    const data = [{ data: { map: () => {} } }];
    return Observable.from(data);
  }
  exportToExcel(gridName, data) {}
}
class MockGridPageColumnsService {
  setCheckboxColumn() {
    return {};
  }
  getActionColumn() {
    return {};
  }
}
class MockInboundRecordsApiService {
  // deleteRecord(recordId: any): Observable<any> {return new Observable<any>();};
  validateRecords(ids: any[]): Observable<any> {
    return new Observable<any>();
  }
  startFiling(ids: any[]): Observable<any> {
    return new Observable<any>();
  }
  canEditFilingHeader(id: number): Observable<any> {
    return new Observable<any>();
  }
  undo(id: number): Observable<any> {
    return new Observable<any>();
  }
}
class MockModalService {
  confirm(data) {
    return Promise.resolve(true);
  }
}
class MockInboundRecordsService {
  private recordIds: any[] = [];
  private filingHeaderId = 0;
  clear() {
    this.recordIds = [];
  }

  setIdentifiersForFiling(filingHeaderId: number, ids: any[]) {
    this.recordIds = ids;
    this.filingHeaderId = filingHeaderId;
  }

  getRecordIdsForFiling() {
    return this.recordIds;
  }
}
class MockInboundRecordsValidator {
  private validRecordsSource = new BehaviorSubject<boolean>(false);
  public validRecordsResult = this.validRecordsSource.asObservable();
  validateSelectedRecords(ids: any[]) {}
}
describe('PipelineListComponent', () => {
  let component: PipelineListComponent;
  let fixture: ComponentFixture<PipelineListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        PipelineListComponent,
        NgxDatatableStubComponent,
        GridToolbarStubComponent,
        GridCountResultsStubComponent,
        GridSelectionToolbarStubComponent,
        FiltersPanelStubComponent,
        ColoredLabelStubComponent,
        IconTooltipStubComponent,
        ReviewAndFileInboundRecordsStubComponent
      ],
      imports: [
        RouterTestingModule.withRoutes([
          {
            path: 'inbound-bridge/review-and-file',
            component: ReviewAndFileInboundRecordsStubComponent
          }
        ])
      ],
      providers: [
        ChangeDetectorRef,
        { provide: GridService, useClass: MockGridService },
        { provide: EventsService, useClass: MockEventsService },
        { provide: GridPageApiService, useClass: MockGridPageApiService },
        {
          provide: GridPageColumnsService,
          useClass: MockGridPageColumnsService
        },
        { provide: GridStorageService, useClass: MockGridStorageService },
        {
          provide: InboundRecordsApiService,
          useClass: MockInboundRecordsApiService
        },
        { provide: ModalService, useClass: MockModalService },
        { provide: InboundRecordsService, useClass: MockInboundRecordsService },
        {
          provide: InboundRecordsValidator,
          useClass: MockInboundRecordsValidator
        }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(PipelineListComponent);
    component = fixture.componentInstance;
  }));

  it('should be created', () => {
    expect(component).toBeDefined();
  });
});
