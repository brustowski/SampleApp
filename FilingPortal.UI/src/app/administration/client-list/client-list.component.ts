import { Component, OnInit, NgZone, ChangeDetectorRef } from '@angular/core';
import { GridPageCtrl } from '../../common/grid/grid-page/grid-page-ctrl';
import { GridPageApiService } from '../../common/grid/services/grid-page-api.service';
import { GridPageColumnsService } from '../../common/grid/services/grid-page-columns.service';
import { GridService } from '../../common/grid/services/grid.service';
import { GridStorageService } from '../../common/grid/services/grid-storage.service';
import { gridPathAPIs, gridNames } from '../../utils';
import { EventsService } from '@common/services';
import { LayoutService } from '@common/services/layout.service';

@Component({
  selector: 'lxft-client-list',
  templateUrl: './client-list.component.html'
})
export class ClientListComponent extends GridPageCtrl implements OnInit {

  constructor(
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.autoHeight = true;
    this.paginationOptions.title = 'Clients';
    this.paginationOptions.pathForApi = gridPathAPIs.clients;
    this.paginationOptions.gridName = gridNames.clients;
    this.paginationOptions.filterConfigName = gridNames.clients;
    this.initGridConfigAndData();
  }

}
