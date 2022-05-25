import { Permissions } from '@common/models';

export interface NavigationTabModel {
    title: string;
    url: string;
    permissions: (Permissions | number)[];
}

export interface NavigationTabConfig {
    cssClass: string;
    tabs: NavigationTabModel[];
}