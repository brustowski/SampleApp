import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoCreateListComponent } from './autocreate-list.component';
import { Component, Input, ChangeDetectorRef } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { PageParams } from '../../common/grid/models/page-params';
import { GridPageApiService } from '../../common/grid/services/grid-page-api.service';
import { GridPageColumnsService } from '../../common/grid/services/grid-page-columns.service';
import { GridService } from '../../common/grid/services/grid.service';
import { GridStorageService } from '../../common/grid/services/grid-storage.service';

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

@Component({ selector: 'lxft-loader', template: '' })
class LoaderStubComponent {

}

class MockGridService {
  getPageParams(paginationOptions: any, sortOptions: any, filtersOptions: any[]) { return <PageParams>{}; }
}
class MockEventsService { }
class MockGridStorageService { }
class MockGridPageApiService {
  getGridColumnsConfig(gridName) {
    return Observable.from([]);
  }
}
class MockGridPageColumnsService {
  setCheckboxColumn() { return {}; }
  getActionColumn() { return {}; }
}

describe('ClientListComponent', () => {
  let component: AutoCreateListComponent;
  let fixture: ComponentFixture<AutoCreateListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AutoCreateListComponent,
        LoaderStubComponent,
        NgxDatatableStubComponent,
        GridToolbarStubComponent,
        GridCountResultsStubComponent,
        GridSelectionToolbarStubComponent,
        FiltersPanelStubComponent],
      providers: [
        ChangeDetectorRef,
        { provide: GridService, useClass: MockGridService },
        { provide: GridPageApiService, useClass: MockGridPageApiService },
        { provide: GridPageColumnsService, useClass: MockGridPageColumnsService },
        { provide: GridStorageService, useClass: MockGridStorageService },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoCreateListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
