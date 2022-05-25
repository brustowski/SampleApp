import { Injectable } from '@angular/core';
import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page, PageBuilder } from '@common/grid/models';
import { ReconGridNames, ReconPermissions, ReconPageConfigNames } from '@common/models';
import { BaseGridsConfigurationService } from '@common/abstract';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService extends BaseGridsConfigurationService {

  constructor() {
    super();
  }

  protected initGrids(): Map<string, Page> {
    const builder = new PageBuilder();
    const grids = new Map<string, Page>();
    grids.set('cargowise-report',
      builder
      .create()
      .title('CargoWise Report')
      .pathForApi('recon/cargowise')
      .gridName(ReconGridNames.CargoWiseRecords)
      .filterConfigName(ReconGridNames.CargoWiseRecords)
      .build());

      grids.set('fta',
      builder
      .create()
      .title('FTA Recon')
      .pathForApi('recon/fta')
      .gridName(ReconGridNames.FtaRecords)
      .filterConfigName(ReconGridNames.FtaRecords)
      .build());

      grids.set('value',
      builder
      .create()
      .title('Value Recon')
      .pathForApi('recon/value')
      .gridName(ReconGridNames.ValueRecords)
      .filterConfigName(ReconGridNames.ValueRecords)
      .build());
      return grids;
  }

  protected initTabs(): Map<string, NavigationTabConfig> {
    const tabs = new Map<string, NavigationTabConfig>();
    tabs.set('main', {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'cargowise-report', title: 'CargoWise Report', permissions: [ReconPermissions.ViewInboundRecord] },
        { url: 'fta', title: 'FTA Recon', permissions: [ReconPermissions.ViewInboundRecord] },
        { url: 'value', title: 'Value Recon', permissions: [ReconPermissions.ViewInboundRecord] },
      ]
    });

    return tabs;
  }

  protected initPageActions(): Map<string, string> {
    const pageActions = new Map<string, string>();

    pageActions.set('cargowise-report', ReconPageConfigNames.InboundViewPageActions);
    pageActions.set('fta', ReconPageConfigNames.FtaViewPageActions);
    pageActions.set('value', ReconPageConfigNames.ValueViewPageActions);

    return pageActions;
  }
}
