import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';

import { locationPath, controllerPath } from '@app/utils';

import { InboundTypeWatcherService } from './inbound-type-watcher.service';

import { InboundType } from '@inbound/models';
import { Page, PageBuilder } from '@common/grid/models';
import {
  GridNames,
  UsExpRailGridNames,
  CanadaImpTruckGridNames,
  ZonesInBondGridNames,
  IsfGridNames
} from '@common/models';
import { ZonesEntry06GridNames } from '@common/models/zones-entry-models';
import { ZonesFtz214GridNames } from '@common/models/zones-ftz214-models';

interface Map<T> {
  [key: string]: T;
}

@Injectable()
export class InboundConfigurationService {
  private uniqueSuffix = 'unique';
  private singleFilingSuffix = 'singleFiling';
  private consoldatedManifestSuffix = 'manifestData';
  private apiPath: Map<string>;
  private mvcPath: Map<string>;
  private pageConfig: Map<Page>;
  private fieldsRecalculationPath: Map<string> = {};
  private filingHeadersConfirmationPath: Map<string> = {};
  private manualTabTitleConfiguration: Map<string> = {};
  private excludeAvailabilityConfiguration: Map<boolean> = {};

  private get inboundType(): InboundType {
    return this.inboundTypeWatcherService.inboundType;
  }

  constructor(private inboundTypeWatcherService: InboundTypeWatcherService) {
    this.pageInit();
    this.apiPath = {};
    this.apiPath[InboundType.Rail] = `${locationPath}/inbound/rail`;
    this.apiPath[InboundType.Truck] = `${locationPath}/inbound/truck`;
    this.apiPath[InboundType.TruckExport] = `${locationPath}/export/truck`;
    this.apiPath[InboundType.Pipeline] = `${locationPath}/inbound/pipeline`;
    this.apiPath[InboundType.Vessel] = `${locationPath}/inbound/vessel`;
    this.apiPath[InboundType.VesselExport] = `${locationPath}/export/vessel`;
    this.apiPath[InboundType.Inbond] = `${locationPath}/zones/in-bond`;
    this.apiPath[InboundType.ZonesEntry] = `${locationPath}/zones/entry-06`;
    this.apiPath[InboundType.ZonesFtz214] = `${locationPath}/zones/ftz-214`;
    this.apiPath[InboundType.CanadaTruckImport] = `${locationPath}/canada-imp-truck`;
    this.apiPath[InboundType.Isf] = `${locationPath}/isf`;
    this.apiPath[InboundType.RailExport] = `${locationPath}/us/export/rail`;

    this.mvcPath = {};
    this.mvcPath[InboundType.Rail] = `${controllerPath}mvc/filing`;
    this.mvcPath[InboundType.Truck] = `${controllerPath}mvc/truckfiling`;
    this.mvcPath[InboundType.TruckExport] = `${controllerPath}mvc/export/truck/filing`;
    this.mvcPath[InboundType.Pipeline] = `${controllerPath}mvc/pipelinefiling`;
    this.mvcPath[InboundType.Vessel] = `${controllerPath}mvc/vesselfiling`;
    this.mvcPath[InboundType.VesselExport] = `${controllerPath}mvc/export/vessel/filing`;
    this.mvcPath[InboundType.Inbond] = `${controllerPath}mvc/zones/in-bond/filing`;
    this.mvcPath[InboundType.ZonesEntry] = `${controllerPath}mvc/zones/entry-06/filing`;
    this.mvcPath[InboundType.ZonesFtz214] = `${controllerPath}mvc/zones/ftz-214/filing`;
    this.mvcPath[InboundType.CanadaTruckImport] = `${controllerPath}mvc/canada/import/truck/filing`;
    this.mvcPath[InboundType.Isf] = `${controllerPath}mvc/isf/filing`;
    this.mvcPath[InboundType.RailExport] = `${controllerPath}mvc/us/export/rail/filing`;

    this.fieldsRecalculationPath = {};
    this.fieldsRecalculationPath[InboundType.Rail] = `${this.apiPath[InboundType.Rail]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.Pipeline] = `${this.apiPath[InboundType.Pipeline]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.Vessel] = `${this.apiPath[InboundType.Vessel]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.VesselExport] = `${this.apiPath[InboundType.VesselExport]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.CanadaTruckImport] = `${this.apiPath[InboundType.CanadaTruckImport]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.Inbond] = `${this.apiPath[InboundType.Inbond]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.ZonesEntry] = `${this.apiPath[InboundType.ZonesEntry]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.ZonesFtz214] = `${this.apiPath[InboundType.ZonesFtz214]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.Isf] = `${this.apiPath[InboundType.Isf]}/filing/process_changes`;
    this.fieldsRecalculationPath[InboundType.RailExport] = `${this.apiPath[InboundType.RailExport]}/filing/process_changes`;

    this.filingHeadersConfirmationPath[InboundType.RailExport] = `${this.apiPath[InboundType.RailExport]}/filing/confirm`;

    this.manualTabTitleConfiguration = {};
    this.manualTabTitleConfiguration[InboundType.CanadaTruckImport] = 'B3N Data';
    this.manualTabTitleConfiguration[InboundType.Inbond] = '7512 Data';
    this.manualTabTitleConfiguration[InboundType.ZonesFtz214] = '214 Data';
    this.excludeAvailabilityConfiguration = {};
    this.excludeAvailabilityConfiguration[InboundType.Isf] = true;
  }

  pageInit(): any {
    const pageBuilder = new PageBuilder();
    this.pageConfig = {};

    this.pageConfig[InboundType.Rail] = pageBuilder
      .create()
      .title('Imports - Rail - US')
      .pathForApi(`inbound/rail`)
      .gridName(GridNames.InboundRecords)
      .filterConfigName(GridNames.InboundRecords)
      .build();
    this.pageConfig[InboundType.Truck] = pageBuilder
      .create()
      .title('Imports - Truck - US')
      .pathForApi(`inbound/truck`)
      .gridName(GridNames.TruckInboundRecords)
      .filterConfigName(GridNames.TruckInboundRecords)
      .build();
    this.pageConfig[InboundType.Pipeline] = pageBuilder
      .create()
      .title('Imports - Pipeline - US')
      .pathForApi(`inbound/pipeline`)
      .gridName(GridNames.PipelineInboundRecords)
      .filterConfigName(GridNames.PipelineInboundRecords)
      .build();
    this.pageConfig[InboundType.TruckExport] = pageBuilder
      .create()
      .title('Exports - Truck - US')
      .pathForApi(`export/truck`)
      .gridName(GridNames.TruckExportRecords)
      .filterConfigName(GridNames.TruckExportRecords)
      .build();
    this.pageConfig[InboundType.Vessel] = pageBuilder
      .create()
      .title('Import - Vessel - US')
      .pathForApi(`inbound/vessel`)
      .gridName(GridNames.VesselImportRecords)
      .filterConfigName(GridNames.VesselImportRecords)
      .build();
    this.pageConfig[InboundType.VesselExport] = pageBuilder
      .create()
      .title('Exports - Vessel - US')
      .pathForApi(`export/vessel`)
      .gridName(GridNames.VesselExportRecords)
      .filterConfigName(GridNames.VesselExportRecords)
      .build();
    this.pageConfig[InboundType.Inbond] = pageBuilder
      .create()
      .title('Zones - In-Bond')
      .pathForApi(`zones/in-bond`)
      .gridName(ZonesInBondGridNames.InbondRecords)
      .filterConfigName(ZonesInBondGridNames.InbondRecords)
      .build();
      this.pageConfig[InboundType.ZonesEntry] = pageBuilder
        .create()
        .title('Zones - Entry 06')
        .pathForApi(`zones/entry-06`)
        .gridName(ZonesEntry06GridNames.InboundRecords)
        .filterConfigName(ZonesEntry06GridNames.InboundRecords)
        .build();
      this.pageConfig[InboundType.ZonesFtz214] = pageBuilder
        .create()
        .title('Zones - FTZ 214')
        .pathForApi(`zones/ftz-214`)
        .gridName(ZonesFtz214GridNames.InboundRecords)
        .filterConfigName(ZonesFtz214GridNames.InboundRecords)
        .build();
      this.pageConfig[InboundType.CanadaTruckImport] = pageBuilder
      .create()
      .title('Imports - Truck - CA')
      .pathForApi(`canada-imp-truck`)
      .gridName(CanadaImpTruckGridNames.InboundRecords)
      .filterConfigName(CanadaImpTruckGridNames.InboundRecords)
      .build();
    this.pageConfig[InboundType.Isf] = pageBuilder
      .create()
      .title('ISF')
      .pathForApi(`isf`)
      .gridName(IsfGridNames.InboundRecords)
      .filterConfigName(IsfGridNames.InboundRecords)
      .build();
    this.pageConfig[InboundType.RailExport] = pageBuilder
      .create()
      .title('Rail Export')
      .pathForApi(`us/export/rail`)
      .gridName(UsExpRailGridNames.InboundRecords)
      .filterConfigName(UsExpRailGridNames.InboundRecords)
      .build();


    this.pageConfig[InboundType.Rail + this.uniqueSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/rail/unique-data`)
      .gridName(GridNames.InboundRecordsUniqueData)
      .filterConfigName(GridNames.InboundRecordsUniqueData)
      .enableSummaryRows(true)
      .build();
    this.pageConfig[InboundType.Truck + this.uniqueSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/truck/unique-data`)
      .gridName(GridNames.TruckInboundUniqueDataGrid)
      .filterConfigName(GridNames.TruckInboundUniqueDataGrid)
      .build();
    this.pageConfig[InboundType.Pipeline + this.uniqueSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/pipeline/unique-data`)
      .gridName(GridNames.PipelineInboundUniqueDataGrid)
      .filterConfigName(GridNames.PipelineInboundUniqueDataGrid)
      .build();

    this.pageConfig[InboundType.Rail + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/rail/single-filing-grid`)
      .gridName(GridNames.RailSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.Rail + this.singleFilingSuffix + this.consoldatedManifestSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/rail/single-filing-grid/manifest-data`)
      .gridName(GridNames.RailManifestDataGrid)
      .build();
    this.pageConfig[InboundType.Truck + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/truck/single-filing-grid`)
      .gridName(GridNames.TruckSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.Pipeline + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/pipeline/single-filing-grid`)
      .gridName(GridNames.PipelineSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.Vessel + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`inbound/vessel/single-filing-grid`)
      .gridName(GridNames.VesselSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.TruckExport + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`export/truck/single-filing-grid`)
      .gridName(GridNames.TruckExportSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.VesselExport + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`export/vessel/single-filing-grid`)
      .gridName(GridNames.VesselExportSingleFilingGrid)
      .build();
    this.pageConfig[InboundType.CanadaTruckImport + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`canada/import/truck/filing-grid`)
      .gridName(CanadaImpTruckGridNames.FilingGrid)
      .build();
    this.pageConfig[InboundType.Inbond + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`zones/in-bond/filing-grid`)
      .gridName(ZonesInBondGridNames.FilingGrid)
      .build();
    this.pageConfig[InboundType.ZonesEntry + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`zones/entry-06/filing-grid`)
      .gridName(ZonesEntry06GridNames.FilingGrid)
      .build();
    this.pageConfig[InboundType.ZonesFtz214 + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`zones/ftz-214/filing-grid`)
      .gridName(ZonesFtz214GridNames.FilingGrid)
      .build();
    this.pageConfig[InboundType.Isf + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`isf/filing-grid`)
      .gridName(IsfGridNames.FilingGrid)
      .build();
    this.pageConfig[InboundType.RailExport + this.singleFilingSuffix] = pageBuilder
      .create()
      .pathForApi(`us/export/rail/filing-grid`)
      .gridName(UsExpRailGridNames.FilingGrid)
      .build();

    // Rail export containers grid on Review and File
    this.pageConfig[InboundType.RailExport + 'containers'] = pageBuilder
      .create()
      .pathForApi(`us/export/rail/containers-grid`)
      .gridName(UsExpRailGridNames.ContainersGrid)
      .build();
  }

  private getApiPathByType(type: InboundType): string {
    return this.apiPath[type];
  }

  public getMvcPathByType(type: InboundType): string {
    return this.mvcPath[type];
  }

  getApiPath(): Observable<string> {
    return of(this.getApiPathByType(this.inboundType));
  }

  getMvcPath(): Observable<string> {
    return of(this.getMvcPathByType(this.inboundType));
  }

  getPageConfiguration(): Page {
    return this.pageConfig[this.inboundType];
  }

  getFilingRecordsPageConfiguration(): Page {
    return this.pageConfig[this.inboundType + this.uniqueSuffix];
  }

  getSingleFilingPageConfiguration(): Page {
    return this.pageConfig[this.inboundType + this.singleFilingSuffix];
  }

  getConsolidatedFilingManifestDataGridConfiguration(): Page {
    return this.pageConfig[this.inboundType + this.singleFilingSuffix + this.consoldatedManifestSuffix];
  }

  getFieldRecalculationPath(): string {
    return this.fieldsRecalculationPath[this.inboundType];
  }

  getManualTabTitle(): string {
    return this.manualTabTitleConfiguration[this.inboundType] || '7501 Data';
  }

  getInboundType(): InboundType {
    return this.inboundType;
  }

  getPageConfig(configName: string): Page {
    return this.pageConfig[this.inboundType + configName];
  }

  getFilingHeaderConfirmationPath(): string {
    return this.filingHeadersConfirmationPath[this.inboundType];
  }

  getExcludeAvailable(): boolean {
    return this.excludeAvailabilityConfiguration[this.inboundType];
  }

  getReviewGridImportPath(): string {
    return `${locationPath}/imports/form-data`;
  }

  getReviewGridExportPath(): string {
    return `${locationPath}/reports/form-data`;
  }
}
