import { Component, Inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import * as R from 'ramda';

import { IGridsConfigurationService } from '@common/interfaces';

import { PageParams } from '@common/grid/models';
import { ReconReportNames } from '@common/models/recon-models';

import { FiltersStorageService } from '@common/filters/services';
import { GridPageApiService, GridService, GridStorageService } from '@common/grid/services';
import { ReconApiService } from '../services/recon-api.service';

@Component({
  selector: 'lxft-value-recon-export-button',
  templateUrl: './value-recon-export-button.component.html',
})
export class ValueReconExportButtonComponent implements OnInit {
  private pageName: string;

  public reports: { name: string, title: string }[] = [
    { name: ReconReportNames.ValueReconEntryReport, title: 'Value recon entry' }
    , { name: ReconReportNames.ValueReconEntryLineReport, title: 'Value recon entry line' }
  ];

  constructor(
    private activatedRoute: ActivatedRoute,
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private api: GridPageApiService,
    private reconApi: ReconApiService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected filtersStorageService: FiltersStorageService,
    private toastr: ToastrService,
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
    const pageParams: PageParams = this.gridService.getPageParams(paginationOptions, sortOptions, filtersOptions);
    this.reconApi.exportabilityCheck(pageParams).subscribe(result => {
      if (result !== '') {
        this.toastr.warning(result);
      } else {
        this.api.exportToExcel(reportName, pageParams);
      }
    });
  }
}
