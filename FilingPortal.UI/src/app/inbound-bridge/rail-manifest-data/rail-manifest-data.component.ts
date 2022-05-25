import { Component, OnInit, ChangeDetectorRef, Input } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { GridService, GridStorageService, GridPageColumnsService, GridPageApiService } from '@common/grid/services';
import { EventsService, LayoutService } from '@common/services';
import { InboundConfigurationService } from '@inbound/services';
import { Filter } from '@common/filters/models';

@Component({
  selector: 'lxft-rail-manifest-data',
  templateUrl: './rail-manifest-data.component.html'
})
export class RailManifestDataComponent extends GridPageCtrl implements OnInit {

  @Input() filingHeaderId: number;

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

    this.paginationOptions = this.configuration.getConsolidatedFilingManifestDataGridConfiguration();

    const filter = new Filter();
    filter.fieldName = 'FilingHeaderId';
    filter.value = [this.filingHeaderId];
    filter.operand = 'equals';

    this.filtersOptions = [filter];
    this.initGridConfigAndData();
  }

  getRowId(row: any): number {
    return row['Id'];
  }

}
