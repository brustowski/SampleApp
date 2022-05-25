import { Permissions } from './models';

export class MenuItem {
    title: string;
    url: string;
    children?: MenuItem[];
    permissions?: number[];
    iconClass: string;
    notSelectOnChildActivation?: boolean;
}
