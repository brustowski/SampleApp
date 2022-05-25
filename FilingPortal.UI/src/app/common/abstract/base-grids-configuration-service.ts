import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page } from '@common/grid/models';
import { IGridsConfigurationService } from '@common/interfaces';

export abstract class BaseGridsConfigurationService implements IGridsConfigurationService {
  private tabs: Map<string, NavigationTabConfig> = new Map<string, NavigationTabConfig>();
  private grids: Map<string, Page> = new Map<string, Page>();
  private pageActions: Map<string, string> = new Map<string, string>();
  constructor() {
    this.tabs = this.initTabs();
    this.grids = this.initGrids();
    this.pageActions = this.initPageActions();
  }
  protected abstract initTabs(): Map<string, NavigationTabConfig>;
  protected abstract initGrids(): Map<string, Page>;
  protected abstract initPageActions(): Map<string, string>;

  getTabsConfig(value: string): NavigationTabConfig {
    return this.tabs.get(value);
  }
  getGridConfig(value: string): Page {
    return this.grids.get(value);
  }
  getPageActionsConfig(value: string): string {
    return this.pageActions.get(value);
  }
}
