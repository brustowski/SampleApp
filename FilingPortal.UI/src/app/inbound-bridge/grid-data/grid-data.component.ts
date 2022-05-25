import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { GridService, GridStorageService, GridPageColumnsService, GridPageApiService } from '@common/grid/services';
import { EventsService, LayoutService } from '@common/services';
import { InboundConfigurationService } from '@inbound/services';
import { Filter } from '@common/filters/models';

@Component({
  selector: 'lxft-grid-data',
  templateUrl: './grid-data.component.html'
})
export class GridDataComponent extends GridPageCtrl implements OnInit {

  @Input() filters: Filter[] = [];
  @Input() pageConfig: string;
  @Input() rowIdColumnName: string = 'Id';

  constructor(
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected events: EventsService,
    protected columnsService: GridPageColumnsService,
    protected api: GridPageApiService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private configuration: InboundConfigurationService,
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);
  }

  ngOnInit() {
    super.ngOnInit();

    this.paginationOptions = this.configuration.getPageConfig(this.pageConfig);
    this.filtersOptions = this.filters;
    this.initGridConfigAndData();
  }

  getRowId(row: any): number {
    return row[this.rowIdColumnName];
  }

}
