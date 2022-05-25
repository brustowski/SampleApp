import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { NavigationTabConfig, NavigationTabModel } from './navigation-tab.model';
import { AuthenticationService } from '@common/services';

@Component({
  selector: 'lxft-navigation-tabs',
  templateUrl: './navigation-tabs.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavigationTabsComponent implements OnInit {

  @Input() navConfig: NavigationTabConfig;

  navClass: string;
  tabs: NavigationTabModel[] = [];

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    if (this.navConfig) {
      this.navClass = this.navConfig.cssClass;
      this.navConfig.tabs.forEach(tab => {
        this.authenticationService.hasPermissions(tab.permissions).subscribe(hasPermissions => {
          if (hasPermissions) {
            this.tabs.push(tab);
          }
        });
      });
    }
  }
}