import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription, Observable } from 'rxjs';

import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { NavigationTabConfig } from '@common/navigation-tabs';
import { IGridsConfigurationService } from '@common/interfaces';

import { AuthenticationService } from '@common/services';
import { ReconService } from '../services/recon.service';
import { ReconReportNames } from '@common/models';

@Component({
  selector: 'app-recon',
  templateUrl: './recon.component.html'
})
export class ReconComponent implements OnInit, OnDestroy {
  public navConfig: NavigationTabConfig;
  public pageTitle: string;
  public dataSubscription: Subscription;
  public urlSubscription: Subscription;
  public currentComponent: GridPageCtrl;
  public isCargowiseExportVisible$: Observable<boolean>;
  public isFtaExportVisible$: Observable<boolean>;
  public isValueExportVisible$: Observable<boolean>;

  constructor(
    private activatedRoute: ActivatedRoute,
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private reconService: ReconService
  ) { }


  ngOnInit() {
    this.isCargowiseExportVisible$ = this.reconService.availableActions$.map(actions => actions && actions.CargowiseExport);
    this.isFtaExportVisible$ = this.reconService.availableActions$.map(actions => actions && actions.FtaExport);
    this.isValueExportVisible$ = this.reconService.availableActions$.map(actions => actions && actions.ValueExport);

    this.dataSubscription = this.activatedRoute.data.subscribe((data: { title: string }) => {
      this.pageTitle = data.title;
    });
    this.urlSubscription = this.activatedRoute.url.subscribe(url => {
      this.navConfig = this.configurationService.getTabsConfig(url[0].path);
      if (!this.activatedRoute.firstChild) {
        if (this.navConfig) {
          let found = false;
          let i = 0;
          while (!found && i < this.navConfig.tabs.length) {
            const tab = this.navConfig.tabs[i++];
            this.authenticationService.hasPermissions(tab.permissions).subscribe(hasPermissions => {
              if (hasPermissions) {
                found = true;
                this.router.navigate([tab.url], { relativeTo: this.activatedRoute });
              }
            });
          }
        }
      }
    });
  }

  ngOnDestroy(): void {
    if (this.dataSubscription) {
      this.dataSubscription.unsubscribe();
    }
    if (this.urlSubscription) {
      this.urlSubscription.unsubscribe();
    }
  }

  onActivate(component: GridPageCtrl) {
    this.currentComponent = component;
  }

  onDeactivate() {
    this.currentComponent = null;
  }
}
