import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import * as R from 'ramda';

import { IGridsConfigurationService } from '@common/interfaces';
import { GridPageApiService, GridStorageService, GridService } from '@common/grid/services';
import { FiltersStorageService } from '@common/filters/services';
import { ReconApiService } from '../services/recon-api.service';
import { ModalService } from '@common/services';

import { PageParams } from '@common/grid/models';
import { ToastrService } from 'ngx-toastr';
import { ReconReportNames } from '@common/models';

@Component({
  selector: 'lxft-fta-recon-export-button',
  templateUrl: './fta-recon-export-button.component.html'
})
export class FtaReconExportButtonComponent implements OnInit {
  private pageName: string;

  get name(): string {
    return ReconReportNames.AssociationReconEntryReport;
  }
  get title(): string {
    return 'FTA Recon Entry';
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private api: GridPageApiService,
    private reconApi: ReconApiService,
    private gridService: GridService,
    private gridStorageService: GridStorageService,
    private filtersStorageService: FiltersStorageService,
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

  export() {
    const paginationOptions = this.configurationService.getGridConfig(this.pageName);
    const sortOptions = this.gridStorageService.getSortingFromStorage(paginationOptions.gridName);
    const filtersOptions = this.filtersStorageService.getFromStorage(paginationOptions.gridName);
    const pageParams: PageParams = this.gridService.getPageParams(paginationOptions, sortOptions, filtersOptions);
    this.reconApi.exportabilityCheck(pageParams).subscribe(result => {
      if (result !== '') {
        this.toastr.warning(result);
      } else {
        this.api.exportToExcel(this.name, pageParams);
      }
    });
  }

}
