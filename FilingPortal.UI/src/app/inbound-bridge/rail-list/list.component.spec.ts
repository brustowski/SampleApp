import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InboundBridgeRailListComponent } from './list.component';
import { Component, Input, ChangeDetectorRef } from '@angular/core';

import { InboundRecordsApiService } from '../services/inbound-records-api.service';
import { RouterTestingModule } from '@angular/router/testing';
import { InboundRecordsService } from '../services/inbound-records.service';
import { InboundRecordsValidator } from '../services/inbound-records.validator';
import { tick } from '@angular/core/testing';
import { fakeAsync } from '@angular/core/testing';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { PageParams } from '@common/grid/models';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { GridService, GridPageApiService, GridPageColumnsService, GridStorageService } from '@common/grid/services';
import { EventsService, ModalService } from '@common/services';
import { InboundRecordListActions } from '@inbound/models';

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
  onClear() { }
}

@Component({ selector: 'lxft-filters-panel', template: '' })
class FiltersPanelStubComponent {
  @Input() gridName: string;
  onApply(e) { }
  onReset() { }
}

@Component({ selector: 'app-colored-label', template: '' })
class ColoredLabelStubComponent {
  @Input() title: string;
  @Input() css: string;
}
@Component({ selector: 'app-review-and-file', template: '' })
class ReviewAndFileInboundRecordsStubComponent {
}
@Component({ selector: 'lxft-icon-tooltip', template: '' })
class IconTooltipStubComponent {
  @Input() messages: any;
  @Input() iconType: any;
}



class MockGridService {
  getPageParams(paginationOptions: any, sortOptions: any, filtersOptions: any[]) { return <PageParams>{}; }
}
class MockEventsService { }
class MockGridStorageService { }
class MockGridPageApiService {
  getGridColumnsConfig(gridName: any): Observable<any> {
    const data = [{ data: { map: () => { } } }];
    return Observable.from(data);
  }
  exportToExcel(gridName, data) { }
}
class MockGridPageColumnsService {
  setCheckboxColumn() { return {}; }
  getActionColumn() { return {}; }
}
class MockInboundRecordsApiService {
  // deleteRecord(recordId: any): Observable<any> {return new Observable<any>();};
  validateRecords(ids: any[]): Observable<any> { return new Observable<any>(); }
  startFiling(ids: any[]): Observable<any> { return new Observable<any>(); }
  canEditFilingHeader(id: number): Observable<any> { return new Observable<any>(); }
  undo(id: number): Observable<any> { return new Observable<any>(); }
}
class MockModalService {
  confirm(data) { return Promise.resolve(true); }
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
  validateSelectedRecords(ids: any[]) { }
}
describe('InboundBridgeListComponent', () => {
  let component: InboundBridgeRailListComponent;
  let fixture: ComponentFixture<InboundBridgeRailListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [InboundBridgeRailListComponent,
        NgxDatatableStubComponent,
        GridToolbarStubComponent,
        GridCountResultsStubComponent,
        GridSelectionToolbarStubComponent,
        FiltersPanelStubComponent,
        ColoredLabelStubComponent,
        IconTooltipStubComponent,
        ReviewAndFileInboundRecordsStubComponent],
      imports: [
        RouterTestingModule.withRoutes([
          {
            path: 'imports/review-and-file',
            component: ReviewAndFileInboundRecordsStubComponent
          }]),
      ],
      providers: [
        ChangeDetectorRef,
        { provide: GridService, useClass: MockGridService },
        { provide: EventsService, useClass: MockEventsService },
        { provide: GridPageApiService, useClass: MockGridPageApiService },
        { provide: GridPageColumnsService, useClass: MockGridPageColumnsService },
        { provide: GridStorageService, useClass: MockGridStorageService },
        { provide: InboundRecordsApiService, useClass: MockInboundRecordsApiService },
        { provide: ModalService, useClass: MockModalService },
        { provide: InboundRecordsService, useClass: MockInboundRecordsService },
        { provide: InboundRecordsValidator, useClass: MockInboundRecordsValidator },
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(InboundBridgeRailListComponent);
    component = fixture.componentInstance;
  }));

  it('should be created', () => {
    expect(component).toBeDefined();
  });
  it('should call modal service on delete', () => {
    const modalService = TestBed.get(ModalService);
    spyOn(modalService, 'confirm');
    modalService.confirm.and.returnValue(Promise.resolve(true));
    component.delete(34);
    expect(modalService.confirm).toHaveBeenCalled();
  });

  /*
  //TODO: test for checking subscribing on Observable after Promise
  it('should call api service on delete', () => {
    const modalService = TestBed.get(ModalService);
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'deleteRecord').and.returnValue({ subscribe: () => {} })
    //modalService.confirm.and.returnValue(Promise.resolve(true));
    component.delete(34);
    expect(apiService.deleteRecord).toHaveBeenCalledWith(34);
  });
  */


  it('should return label type for Open mapping status', () => {
    const type = component.getMappingLabelType('Open');
    expect(type).toEqual('open');
  });
  it('should return label type for In Progress mapping status', () => {
    const type = component.getMappingLabelType('In Progress');
    expect(type).toEqual('inprogress');
  });
  it('should return label type for In Review mapping status', () => {
    const type = component.getMappingLabelType('In Review');
    expect(type).toEqual('inreview');
  });
  it('should return label type for Mapped mapping status', () => {
    const type = component.getMappingLabelType('Mapped');
    expect(type).toEqual('mapped');
  });
  it('should return label type for Error mapping status', () => {
    const type = component.getMappingLabelType('Error');
    expect(type).toEqual('error');
  });
  it('should return no label type for null mapping status', () => {
    const type = component.getMappingLabelType(null);
    expect(type).toEqual(null);
  });
  it('should return label type for Open filing status', () => {
    const type = component.getFilingLabelType('Open');
    expect(type).toEqual('open');
  });
  it('should return label type for In Progress filing status', () => {
    const type = component.getFilingLabelType('In Progress');
    expect(type).toEqual('inprogress');
  });
  it('should return label type for Filed filing status', () => {
    const type = component.getFilingLabelType('Filed');
    expect(type).toEqual('filed');
  });
  it('should return label type for Error filing status', () => {
    const type = component.getFilingLabelType('Error');
    expect(type).toEqual('error');
  });
  it('should return no label type for null filing status', () => {
    const type = component.getFilingLabelType(null);
    expect(type).toEqual(null);
  });

  it('should return row identifier for grid', () => {
    const row = { Id: 45, id: 324, name: 'sdf' };
    const id = component.getRowId(row);
    expect(id).toEqual(45);
  });

  it('should return that row can be checked when Select is allowed', () => {
    const row = { Id: 45, Actions: { Select: true } };
    const result = component.canBeChecked(row);
    expect(result).toEqual(true);
  });

  it('should return that row cannot be checked when Select is not allowed', () => {
    const row = { Id: 45, Actions: { Select: false } };
    const result = component.canBeChecked(row);
    expect(result).toEqual(false);
  });

  it('should call api service for validation on start filing', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'startFiling').and.returnValue({ subscribe: () => { } });
    component.validRecords = true;
    component.file();
    expect(apiService.startFiling).toHaveBeenCalled();
  });

  it('should call call validation api on edit filing headers when selected rows and filing header id are defined', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'canEditFilingHeader').and.returnValue({ subscribe: () => { } });
    (component as any).selectedFilingHeaderId = 16;
    (component as any).selectedRows = [{ Id: 1 }, { Id: 2 }, { Id: 3 }];
    component.edit();
    expect(apiService.canEditFilingHeader).toHaveBeenCalled();
  });

  it('should not call validation api on edit filing headers when selected rows are not filled', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'canEditFilingHeader').and.returnValue({ subscribe: () => { } });
    (component as any).selectedFilingHeaderId = 16;
    (component as any).selectedRows = [];
    component.edit();
    expect(apiService.canEditFilingHeader).not.toHaveBeenCalled();
  });

  it('should not call validation api on edit filing headers when filing header id is not filled', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'canEditFilingHeader').and.returnValue({ subscribe: () => { } });
    (component as any).selectedFilingHeaderId = null;
    (component as any).selectedRows = [{ Id: 1 }, { Id: 2 }, { Id: 3 }];
    component.edit();
    expect(apiService.canEditFilingHeader).not.toHaveBeenCalled();
  });

  it('should not call validation api on edit filing headers when both selected rows and filing header id are not filled', () => {
    const apiService = TestBed.get(InboundRecordsApiService);
    spyOn(apiService, 'canEditFilingHeader').and.returnValue({ subscribe: () => { } });
    (component as any).selectedFilingHeaderId = null;
    (component as any).selectedRows = [];
    component.edit();
    expect(apiService.canEditFilingHeader).not.toHaveBeenCalled();
  });

  it('should call validator to validate on selecting records', () => {
    (component as any).rows = [{ Id: 5 }, { Id: 98 }];
    (component as any).selectedRows = [{ Id: 5 }, { Id: 98 }];
    const validator = TestBed.get(InboundRecordsValidator);
    spyOn(validator, 'validateSelectedRecords').and.returnValue({
      subscribe: () => {
        return {
          IsValid: true,
          Actions: { ReviewFile: true, Undo: false, Edit: true },
          RecordErrors: []
        };
      }
    });
    component.onSelect({ selected: [{ Id: 5 }, { Id: 98 }] });
    expect(validator.validateSelectedRecords).toHaveBeenCalled();
  });

  it('should set valid flag and actions after validating selected records', fakeAsync(() => {
    (component as any).rows = [{ Id: 5 }, { Id: 98 }];
    (component as any).selectedRows = [{ Id: 5 }, { Id: 98 }];
    const validator = TestBed.get(InboundRecordsValidator);
    const actions = new InboundRecordListActions();
    actions.ReviewFile = true;
    actions.Undo = false;
    actions.Edit = true;
    spyOn(validator, 'validateSelectedRecords').and.returnValue(Observable.from([{
      IsValid: true,
      Actions: actions,
      RecordErrors: []
    }
    ]));
    component.onSelect({ selected: [{ Id: 5 }, { Id: 98 }] });
    tick();
    expect(component.validRecords).toEqual(true);
    expect(component.availableActions).toEqual(actions);
  }));

  it('should call export grid api when exporting to excel', () => {
    const gridApiService = TestBed.get(GridPageApiService);
    spyOn(gridApiService, 'exportToExcel');

    component.exportList();

    expect(gridApiService.exportToExcel).toHaveBeenCalled();
  });
});
