import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page } from '@common/grid/models';

export interface IGridsConfigurationService {
  getTabsConfig(value: string): NavigationTabConfig;
  getGridConfig(value: string): Page;
  getPageActionsConfig(value: string): string;
}
