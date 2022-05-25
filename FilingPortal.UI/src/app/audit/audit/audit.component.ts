import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs';
import * as R from 'ramda';

import { Page } from '@common/grid/models';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { NavigationTabConfig } from '@common/navigation-tabs';
import { instanceOfGridWithExcelTemplate } from '@common/typeguards/grid-page';
import { FiltersStorageService } from '@common/filters/services/filters-storage.service';
import { IGridsConfigurationService } from '@common/interfaces';
import { AuthenticationService } from '@common/services';
import { GridPageApiService, GridService, GridStorageService } from '@common/grid/services';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-audit',
  templateUrl: './audit.component.html'
})
export class AuditComponent implements OnInit, OnDestroy {
  public navConfig: NavigationTabConfig;
  public pageTitle: string;
  public dataSubscription: Subscription;
  public urlSubscription: Subscription;
  public currentComponent: GridPageCtrl;

  public uploadUrl: string = `/imports/uploads`;
  public get isRuleImportButtonVisible(): boolean {
    return this.IsGridWithExcelTemplate
      && this.currentComponent.availableActions
      && this.currentComponent.availableActions.Import
      && this.currentComponent.paginationOptions.gridName.endsWith('_rules');
  }
  public isImportButtonVisible: boolean = false;
  public IsGridWithExcelTemplate: boolean = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    @Inject('GridsConfigurationService') private configurationService: IGridsConfigurationService,
    private authenticationService: AuthenticationService,
    private router: Router,
    protected api: GridPageApiService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected filtersStorageService: FiltersStorageService,
    protected toastr: ToastrService,
  ) { }

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
    let route = this.activatedRoute;
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

  onActivate(component: GridPageCtrl) {
    this.currentComponent = component;
    this.IsGridWithExcelTemplate = instanceOfGridWithExcelTemplate(this.currentComponent);
  }

  onDeactivate() {
    this.currentComponent = null;
    this.IsGridWithExcelTemplate = false;
  }

  import() {
    if (instanceOfGridWithExcelTemplate(this.currentComponent)) {
      this.currentComponent.fileInput.nativeElement.click();
    }
  }

  downloadTemplate() {
    if (instanceOfGridWithExcelTemplate(this.currentComponent)) {
      this.currentComponent.downloadTemplate();
    }
  }

  onUploadSuccess(): void {
      this.currentComponent.getPage();
  }

  onUploadError($event: any): void {
    this.toastr.error($event);
  }
}
