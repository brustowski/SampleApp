import { Injectable } from '@angular/core';

import { locationPath } from '@app/utils';

import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page, PageBuilder } from '@common/grid/models';
import { PageConfigNames, GridNames, CanadaImpTruckGridNames, IsfGridNames, UsExpRailGridNames } from '@common/models';
import { HttpClient } from '@angular/common/http';
import { ZonesInBondGridNames } from '@common/models/zones-inbond-models';
import { ZonesEntry06GridNames } from '@common/models/zones-entry-models';
import { ZonesFtz214GridNames } from '@common/models/zones-ftz214-models';
@Injectable()
export class RulesConfigurationService {
  private grids: { [key: string]: Page };
  private pageActions: { [key: string]: string };

  constructor(private http: HttpClient) {
    this.initGridPageSettings();
    this.initPageActions();
  }

  getTabs(): NavigationTabConfig {
    return <NavigationTabConfig>{
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'rail-import', title: 'Rail Import' }
        , { url: 'pipeline-import', title: 'Pipeline Import' }
        , { url: 'truck-import', title: 'Truck Import' }
        , { url: 'vessel-import', title: 'Vessel Import' }
        , { url: 'vessel-export', title: 'Vessel Export' }
        , { url: 'truck-export', title: 'Truck Export' }
        , { url: 'zones-inbond', title: 'Zones In-Bond' }
        , { url: 'zones-entry-06', title: 'Zones Entry 06' }
        , { url: 'zones-ftz-214', title: 'Zones FTZ 214'}
        , { url: 'canada-imp-truck', title: 'CA Truck Import' }
        , { url: 'isf', title: 'ISF' }
        , { url: 'us-export-rail', title: 'US Export Rail'}        
      ]
    };
  }

  private initGridPageSettings(): void {
    const builder = new PageBuilder();
    this.grids = {};
    this.grids['rail-import'] = builder
      .create()
      .title('Rail Import')
      .pathForApi('rules/rail/default-values')
      .gridName(GridNames.RailDefaultValues)
      .filterConfigName(GridNames.RailDefaultValues)
      .build();
    this.grids['pipeline-import'] = builder
      .create()
      .title('Pipeline Import')
      .pathForApi('rules/pipeline/default-values')
      .gridName(GridNames.PipelineDefaultValues)
      .filterConfigName(GridNames.PipelineDefaultValues)
      .build();
    this.grids['truck-import'] = builder
      .create()
      .title('Truck Import')
      .pathForApi('rules/truck/default-values')
      .gridName(GridNames.TruckDefaultValues)
      .filterConfigName(GridNames.TruckDefaultValues)
      .build();
    this.grids['vessel-import'] = builder
      .create()
      .title('Vessel Import')
      .pathForApi('rules/vessel/default-values')
      .gridName(GridNames.VesselDefaultValues)
      .filterConfigName(GridNames.VesselDefaultValues)
      .build();
    this.grids['vessel-export'] = builder
      .create()
      .title('Vessel Export')
      .pathForApi('rules/export/vessel/default-values')
      .gridName(GridNames.VesselExportDefaultValues)
      .filterConfigName(GridNames.VesselExportDefaultValues)
      .build();
    this.grids['truck-export'] = builder
      .create()
      .title('Truck Export')
      .pathForApi('rules/export/truck/default-values')
      .gridName(GridNames.TruckExportDefaultValues)
      .filterConfigName(GridNames.TruckExportDefaultValues)
      .build();
    this.grids['canada-imp-truck'] = builder
      .create()
      .title('CA Truck Import')
      .pathForApi('rules/canada/import/truck/default-values')
      .gridName(CanadaImpTruckGridNames.DefaultValues)
      .filterConfigName(CanadaImpTruckGridNames.DefaultValues)
      .build();
    this.grids['zones-inbond'] = builder
      .create()
      .title('Zones In-Bond')
      .pathForApi('rules/zones/in-bond/default-values')
      .gridName(ZonesInBondGridNames.DefaultValues)
      .filterConfigName(ZonesInBondGridNames.DefaultValues)
      .build();
      this.grids['zones-entry-06'] = builder
      .create()
      .title('Zones Entry 06')
      .pathForApi('rules/zones/entry-06/default-values')
      .gridName(ZonesEntry06GridNames.DefaultValues)
      .filterConfigName(ZonesEntry06GridNames.DefaultValues)
      .build();
      this.grids['zones-ftz-214'] = builder
      .create()
      .title('Zones FTZ 214')
      .pathForApi('rules/zones/ftz-214/default-values')
      .gridName(ZonesFtz214GridNames.DefaultValues)
      .filterConfigName(ZonesFtz214GridNames.DefaultValues)
      .build();
    this.grids['isf'] = builder
      .create()
      .title('ISF')
      .pathForApi('rules/isf/default-values')
      .gridName(IsfGridNames.DefaultValues)
      .filterConfigName(IsfGridNames.DefaultValues)
      .build();
    this.grids['us-export-rail'] = builder
      .create()
      .title('US Export Rail')
      .pathForApi('rules/us/export/rail/default-values')
      .gridName(UsExpRailGridNames.DefaultValues)
      .filterConfigName(UsExpRailGridNames.DefaultValues)
      .build();
  }

  getGridPageSettings(name: string): Page {
    return this.grids[name];
  }

  private initPageActions(): void {
    this.pageActions = {};
    this.pageActions['rail-import'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['pipeline-import'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['truck-import'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['vessel-import'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['truck-export'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['canada-imp-truck'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['zones-inbond'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['zones-entry-06'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['zones-ftz-214'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['isf'] = PageConfigNames.ConfigurationPageActions;
    this.pageActions['us-export-rail'] = PageConfigNames.ConfigurationPageActions;
  }

  getPageActionsConfig(value: string): string {
    return this.pageActions[value];
  }

  delete(pathForApi: string, rowId: number): any {
    return this.http
      .post(`${locationPath}/${pathForApi}/delete/${rowId}`, {});
  }

  update(pathForApi: string, row: any): any {
    return this.http
      .post(`${locationPath}/${pathForApi}/update`, row);
  }

  getNew(pathForApi: string): any {
    return this.http
      .get(`${locationPath}/${pathForApi}/getNew`);
  }

  depersonalize(templateRow: any): any {
    const newRow = { ...templateRow };
    newRow.Id = 0;
    delete newRow.options;
    delete newRow.ViewMode;
    return newRow;
  }

  add(pathForApi: string, row: any): any {
    return this.http
      .post(`${locationPath}/${pathForApi}/create`, row);
  }
}
