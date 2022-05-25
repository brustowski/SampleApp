import { Component, OnInit, OnDestroy } from '@angular/core';
import { NavigationTabConfig } from '../../common/navigation-tabs';
import { ActivatedRoute, Router, ParamMap, NavigationEnd } from '@angular/router';
import { RulesConfigurationService } from '../services';
import { AuthenticationService } from '@common/services';
import { GridPageApiService, GridService, GridStorageService } from '@common/grid/services';
import { Page } from '@common/grid/models';
import { FiltersStorageService } from '@common/filters/services/filters-storage.service';
import { filter, map, mergeMap, tap } from 'rxjs/operators';
import { forkJoin, combineLatest, Subscription } from 'rxjs';
import * as R from 'ramda';
import { PageMode } from '@common/models/send-request-model';

@Component({
  selector: 'lxft-rules',
  templateUrl: './rules.component.html'
})
export class RulesComponent implements OnInit, OnDestroy {
  navConfig: NavigationTabConfig;
  pageTitle: string;
  dataSubscription: Subscription;
  urlSubscription: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private configurationService: RulesConfigurationService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute,
    protected api: GridPageApiService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected filtersStorageService: FiltersStorageService
  ) {}

  ngOnInit() {
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

  getPageParams(paginationOptions: Page) {
    const sortOptions = this.gridStorageService.getSortingFromStorage(paginationOptions.gridName);
    const filtersOptions = this.filtersStorageService.getFromStorage(paginationOptions.gridName);
    return this.gridService.getPageParams(paginationOptions, sortOptions, filtersOptions);
  }

  exportList() {
    let route = this.route;
    while (route.firstChild) {
      route = route.firstChild;
    }
    let pageName = route.snapshot.paramMap.get('name');
    if (!pageName) {
      pageName = R.last(route.snapshot.url).path;
    }
    const paginationOptions = this.configurationService.getGridConfig(pageName);
    this.api.exportToExcel(paginationOptions.gridName, this.getPageParams(paginationOptions));
  }
}
