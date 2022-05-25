import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import {FormsModule} from '@angular/forms';

import { RuleListComponent } from './rule-list.component';
import { Component, Input } from '@angular/core';
import {RouterTestingModule} from "@angular/router/testing";

import { GridService } from '../../common/grid/services/grid.service';
import { EventsService } from '../../common/services/events.service';
import { GridPageColumnsService } from '../../common/grid/services/grid-page-columns.service';
import { GridPageApiService } from '../../common/grid/services/grid-page-api.service';
import { GridStorageService } from '../../common/grid/services/grid-storage.service';
import {Observable} from "rxjs";
import {PageParams} from "../../common/grid/models/page-params";
import {NotificationService} from "../../common/notification/notification.service";
import {ModalService} from "../../common/services/modal.service";
import {ValidationResultWithFieldsErrorsViewModel} from "../../common/models/validation-result-with-fields-error-view-model";
import {RulesApiService} from "../services/rules-api.service";

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
  @Input() disabled: any;
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
  @Input() disabled: any;
  onApply(e) { }
  onReset() { }
}

@Component({ selector: 'lxft-field-number', template: '' })
class FieldNumberStubComponent {
  @Input() options: any;
  @Input() value: any;
}
@Component({ selector: 'lxft-field-float-number', template: '' })
class FieldFloatNumberStubComponent {
  @Input() options: any;
  @Input() value: any;
}
@Component({ selector: 'lxft-field-text', template: '' })
class FieldTextStubComponent {
  @Input() options: any;
  @Input() value: any;
}

class MockGridService {
  getPageParams(paginationOptions: any, sortOptions: any, filtersOptions: any[]){return <PageParams>{};}
}
class MockEventsService { }
class MockGridStorageService { }
class MockGridPageApiService {
  getGridColumnsConfig(gridName: any): Observable<any> {
    const data = [{data: {map: ()=>{}}}];
    return Observable.from(data);
  }
  exportToExcel(gridName, data) { }
}
class MockGridPageColumnsService {
  setCheckboxColumn() { return {}; }
  getActionColumn() { return {}; }
}
class MockModalService {
  confirm(data) { return Promise.resolve(true); }
}
class MockNotificationService {
  alert(data) { }
}
class MockRulesApiService {
  // deleteRecord(recordId: any): Observable<any> {return new Observable<any>();};
  deleteRule(pathForApi: string, ruleId: any): Observable<any> { return new Observable<any>(); }
  addRule<TRule>(pathForApi: string, rule: TRule): Observable<any> { return new Observable<any>(); }
  getNewRule<TRule>(pathForApi: string): Observable<any>{ return new Observable<any>(); }
  updateRule<TRule>(pathForApi: string, rule: TRule): Observable<ValidationResultWithFieldsErrorsViewModel> { return new Observable<any>(); }
  createRule<TRule>(pathForApi: string, rule: TRule): Observable<ValidationResultWithFieldsErrorsViewModel> { return new Observable<any>(); }
}
describe('RuleListComponent', () => {
  let component: RuleListComponent;
  let fixture: ComponentFixture<RuleListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RuleListComponent,
        NgxDatatableStubComponent,
        GridToolbarStubComponent,
        GridCountResultsStubComponent,
        GridSelectionToolbarStubComponent,
        FiltersPanelStubComponent,
        FieldNumberStubComponent,
        FieldFloatNumberStubComponent,
        FieldTextStubComponent
      ],
      imports: [FormsModule,
        RouterTestingModule.withRoutes([])
      ],
      providers: [
        { provide: GridService, useClass: MockGridService },
        { provide: EventsService, useClass: MockEventsService },
        { provide: GridPageApiService, useClass: MockGridPageApiService },
        { provide: GridPageColumnsService, useClass: MockGridPageColumnsService },
        { provide: GridStorageService, useClass: MockGridStorageService },
        { provide: ModalService, useClass: MockModalService },
        { provide: NotificationService, useClass: MockNotificationService },
        { provide: RulesApiService, useClass: MockRulesApiService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RuleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
