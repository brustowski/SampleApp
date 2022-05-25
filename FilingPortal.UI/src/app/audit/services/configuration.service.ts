import { Injectable } from '@angular/core';
import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page, PageBuilder } from '@common/grid/models';
import { PageConfigNames, GridNames, Permissions } from '@common/models';
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
    grids.set('consist-sheet',
      builder
      .create()
      .title('Train Consist Sheet')
      .pathForApi('audit/rail/train-consist-sheet')
      .gridName(GridNames.AuditRailTrainConsistSheet)
      .filterConfigName(GridNames.AuditRailTrainConsistSheet)
      .build());
    grids.set('daily-audit',
      builder
      .create()
      .title('Daily Audit')
      .pathForApi('audit/rail/daily-audit')
      .gridName(GridNames.AuditRailDailyAudit)
      .filterConfigName(GridNames.AuditRailDailyAudit)
      .build());
    grids.set('daily-audit-rules',
      builder
      .create()
      .title('Daily Audit Rules')
      .pathForApi('audit/rail/daily-audit-rules')
      .gridName(GridNames.AuditRailDailyAuditRules)
      .filterConfigName(GridNames.AuditRailDailyAuditRules)
      .build());
    grids.set('daily-audit-spi-rules',
      builder
      .create()
      .title('Daily Audit SPI Rules')
      .pathForApi('audit/rail/daily-audit-spi-rules')
      .gridName(GridNames.AuditRailDailyAuditSpiRules)
      .filterConfigName(GridNames.AuditRailDailyAuditSpiRules)
      .build());
      return grids;
  }

  protected initTabs(): Map<string, NavigationTabConfig> {
    const tabs = new Map<string, NavigationTabConfig>();
    tabs.set('rail', {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'consist-sheet', title: 'Rail Consist Sheet', permissions: [Permissions.AuditRailImportTrainConsistSheet] },
        { url: 'daily-audit', title: 'Rail Daily Audit', permissions: [Permissions.AuditRailDailyAudit] },
        { url: 'daily-audit-rules', title: 'Rail Daily Audit Rules', permissions: [Permissions.AuditRailDailyAudit] },
        { url: 'daily-audit-spi-rules', title: 'Rail Daily Audit SPI Rules', permissions: [Permissions.AuditRailDailyAudit] },
      ]
    });

    return tabs;
  }

  protected initPageActions(): Map<string, string> {
    const pageActions = new Map<string, string>();

    pageActions.set('consist-sheet', PageConfigNames.AuditRailTrainConsistSheetPageActions );
    pageActions.set('daily-audit-rules', PageConfigNames.AuditRailDailyAuditRulesPageActions );
    pageActions.set('daily-audit-spi-rules', PageConfigNames.AuditRailDailyAuditRulesPageActions );

    return pageActions;
  }
}
