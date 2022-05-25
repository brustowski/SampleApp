import {
  Component,
  OnInit,
  ViewChild,
  TemplateRef,
  NgZone,
  ChangeDetectorRef
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as R from 'ramda';

import { InboundRecordListActions } from '@inbound/models';

import {
  EventsService,
  ModalService,
  ConfigurationService,
  FileUploadService
} from '@common/services';
import { GridService } from '@common/grid/services/grid.service';
import { GridPageApiService } from '@common/grid/services/grid-page-api.service';
import { GridPageColumnsService } from '@common/grid/services/grid-page-columns.service';
import { GridStorageService } from '@common/grid/services/grid-storage.service';
import {
  InboundRecordsApiService,
  InboundRecordsService,
  InboundConfigurationService,
  SingleFilingService,
  FilingParametersService
} from '@inbound/services';

import { AddUserActionsPropertyToModelHandler } from '@inbound/handlers';

import {
  typeMappHighlightingIcon,
  styleMappHighlightingType,
} from '@app/utils';
import { IconType } from '@common/icon-tooltip';
import { AvailableActions, HighlightingType, IsfPageConfigNames } from '@common/models';
import { GridDataHandler } from '@common/grid/handlers';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { LayoutService } from '@common/services/layout.service';
import { ToastrService } from 'ngx-toastr';
import { DomSanitizer } from '@angular/platform-browser';
import { IsfAddInboundModalComponent } from '@inbound/isf-add-inbound-modal';

@Component({
  selector: 'lxft-isf-list',
  templateUrl: './isf-list.component.html'
})
export class IsfListComponent extends CommonInboundList implements OnInit {
  @ViewChild('notificationIconTmpl')
  public notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  public statusTmpl: TemplateRef<any>;

  protected get actionsTemplate(): TemplateRef<any> {
    return this.actionsTmpl;
  }
  protected get statusTemplate(): TemplateRef<any> {
    return this.statusTmpl;
  }

  public pageAvailableActions: AvailableActions;
  public availableActions = new InboundRecordListActions();
  public isWorking: boolean = false;

  public infoIconType = IconType.Info;

  constructor(
    protected gridService: GridService,
    protected events: EventsService,
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridStorageService: GridStorageService,
    protected zone: NgZone,
    protected route: ActivatedRoute,
    protected router: Router,
    protected toastr: ToastrService,
    protected fileService: FileUploadService,
    protected modal: ModalService,
    private configuration: InboundConfigurationService,
    protected inboundRecordsApiService: InboundRecordsApiService,
    protected inboundRecordsService: InboundRecordsService,
    private configurationService: ConfigurationService,
    protected singleFilingService: SingleFilingService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private filingParametersService: FilingParametersService,
    sanitizer: DomSanitizer
  ) {
    super(
      api,
      columnsService,
      gridService,
      gridStorageService,
      inboundRecordsService,
      inboundRecordsApiService,
      singleFilingService,
      zone,
      route,
      router,
      modal,
      events,
      cdr,
      layoutService,
      sanitizer,
      fileService,
      toastr
    );

    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addUserActionsHandler',
        AddUserActionsPropertyToModelHandler.handler
      )
    );
  }

  ngOnInit() {
    super.ngOnInit();
    this.paginationOptions = this.configuration.getPageConfiguration();
    this.initGridConfigAndData();
    this.setPageAvailableActions();
  }

  setPageAvailableActions(): void {
    this.configurationService
      .getPageActions(IsfPageConfigNames.InboundViewPageActions)
      .subscribe(result => (this.pageAvailableActions = result));
  }

  get massUploadUrl(): string {
    return 'isf/documents-upload';
  }

  updateGridColumns() {
    this.setFirstColumnBorder();
    this.setCheckboxColumns();
    this.setFilingNumberColumn();
    this.setStatusColumns();
    this.setActionsGridColumn();
    this.setNotificationColumn();
  }

  private setFirstColumnBorder() {
    if (this.columns && this.columns.length) {
      this.columns[0] = this.hideColumnBorder(this.columns[0]);
    }
  }

  setCheckboxColumns() {
    const checkboxField = this.columnsService.setCheckboxColumn();
    this.columns.unshift(this.hideColumnBorder(checkboxField));
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(
      this.notificationIconTmpl
    );
    this.columns.push(this.hideColumnBorder(field));
  }

  getIconTypeFor(row: any): IconType {
    const highlightingType = this.getRowHighlighting(row);
    const iconType = typeMappHighlightingIcon.find(
      m => m.highlightingType === highlightingType
    );
    return iconType ? iconType.iconType : IconType.None;
  }

  getRowHighlighting(row: any): HighlightingType {
    return row.HighlightingType
      ? row.HighlightingType
      : HighlightingType.NoHighlighting;
  }

  getRowClass = row => this.getHighlightingStyleForRow(row);

  private getHighlightingStyleForRow(row): string {
    const highlightingType = this.getRowHighlighting(row);
    return styleMappHighlightingType[highlightingType];
  }

  onSelect(event) {
    super.OnSelect(event);
    this.updateSelectActions(this.selectedRows);
    this.validateSelectedRows(this.selectedRows);
    this.refreshGrid();
  }

  validateSelectedRows(selectedRows: any[]): void {
    if (selectedRows.length > 0) {
      const ids = selectedRows.map(s => s.Id);
      this.inboundRecordsApiService
        .getAvailableActions(ids)
        .subscribe(result => {
          this.availableActions = result;
        });
    } else {
      this.clearAvailableActions();
    }
  }

  /**
   * Manual grid refresh
   */
  refreshGrid(): void {
    this.dataHandlers.handleMultiple(this.rows);
    this.rows = R.clone(this.rows);
  }

  clearAvailableActions() {
    this.availableActions = new InboundRecordListActions();
  }

  clearSelected() {
    super.clearSelected();
    this.refreshGrid();
    this.clearAvailableActions();
  }

  exportList() {
    this.api.exportToExcel(
      this.paginationOptions.gridName,
      this.getPageParams()
    );
  }

  downloadTempalte(): void {
    this.api.downloadTemplate(this.paginationOptions.gridName);
  }

  protected goTo(filingHeaderIds: number[], recordsIds: number[] = []): void {
    this.filingParametersService.clear();
    this.filingParametersService.filingHeaderIds = filingHeaderIds;
    this.router.navigate(['review-and-file'], {
      relativeTo: this.route
    });
  }

  view(): void {
    const filingHeaderIds: number[] = this.selectedRows.map(
      r => r.FilingHeaderId
    );
    this.inboundRecordsApiService
      .getRecordIds(filingHeaderIds)
      .subscribe(() => {
        this.filingParametersService.clear();
        this.filingParametersService.filingHeaderIds = filingHeaderIds;
        this.router.navigate(['view'], {
          relativeTo: this.route
        });
      });
  }

  /**
 * Opens modal dialog to add new import record
 */
  add() {
    const data = {
      windowClass: 'modal-m',
    };
    this.modal.open(IsfAddInboundModalComponent, data).then((result: { ids: number[], gotoFiling: boolean }) => {
      if (!result.gotoFiling) {
        this.getPage();
      } else {
        this.inboundRecordsApiService
          .startFiling(result.ids)
          .subscribe(filingHeaderIds => {
            this.goTo(filingHeaderIds, result.ids);
          });
      }
    }).catch(() => { });
  }
  /**
 * Opens initial record management form
 * @param row selected row
 * @param viewMode view mode
 */
  openInitial(row: any, viewMode: boolean = false) {
    const data = {
      windowClass: 'modal-m',
      rowData: row,
      isExport: true,
      viewMode: viewMode,
    };
    this.modal.open(IsfAddInboundModalComponent, data).then((result: { ids: number[], gotoFiling: boolean }) => {
      if (!result.gotoFiling) {
        this.getPage();
      } else {
        this.inboundRecordsApiService
          .startFiling(result.ids)
          .subscribe(filingHeaderIds => {
            this.goTo(filingHeaderIds, result.ids);
          });
      }
    }).catch(() => { });
  }
}
