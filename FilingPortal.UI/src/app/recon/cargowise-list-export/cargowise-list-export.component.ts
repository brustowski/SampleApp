import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import * as R from 'ramda';

import { ReconReportNames } from '@common/models';
import { IGridsConfigurationService } from '@common/interfaces';
import { GridPageApiService, GridStorageService, GridService } from '@common/grid/services';
import { FiltersStorageService } from '@common/filters/services';

@Component({
  selector: 'lxft-cargowise-list-export',
  templateUrl: './cargowise-list-export.component.html'
})
export class CargowiseListExportComponent implements OnInit {
  private pageName: string;

  public reports: { name: string, title: string }[] = [
    { name: ReconReportNames.CargoWiseInternal, title: 'Internal' }
    , { name: ReconReportNames.CargoWiseClientFta, title: 'Client FTA' }
    , { name: ReconReportNames.CargoWiseClientValue, title: 'Client Value' }
  ];

  constructor(
    private activatedRoute: ActivatedRoute,
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private api: GridPageApiService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected filtersStorageService: FiltersStorageService,
  ) { }

  ngOnInit() {
    let route = this.activatedRoute;
    while (route.firstChild) {
      route = route.firstChild;
    }
    let pageName = route.snapshot.paramMap.get('name');
    if (!pageName) {
      pageName = R.last(route.snapshot.url).path;
    }
    this.pageName = pageName;
  }

  exportList(reportName: string) {
    const paginationOptions = this.configurationService.getGridConfig(this.pageName);
    const sortOptions = this.gridStorageService.getSortingFromStorage(paginationOptions.gridName);
    const filtersOptions = this.filtersStorageService.getFromStorage(paginationOptions.gridName);
    this.api.exportToExcel(reportName, this.gridService.getPageParams(paginationOptions, sortOptions, filtersOptions));
  }

}
