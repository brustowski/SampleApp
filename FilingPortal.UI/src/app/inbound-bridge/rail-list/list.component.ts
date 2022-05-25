import { Component, OnInit, TemplateRef, ViewChild, NgZone, ChangeDetectorRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/do';

import { GridService } from '@common/grid/services/grid.service';
import { EventsService } from '@common/services/events.service';
import { GridPageApiService } from '@common/grid/services/grid-page-api.service';
import { GridPageColumnsService } from '@common/grid/services/grid-page-columns.service';
import { ModalService } from '@common/services/modal.service';

import { ManifestComponent } from '@inbound/manifest';

import { typeMappHighlightingIcon, styleMappHighlightingType } from '@app/utils';
import { GridDataHandler } from '@common/grid/handlers';
import { IconType } from '@common/icon-tooltip';
import { InboundRecordListActions } from '@inbound/models';
import { GridStorageService } from '@common/grid/services/grid-storage.service';
import {
  InboundRecordsValidator,
  InboundRecordsApiService,
  InboundRecordsService,
  InboundConfigurationService,
  SingleFilingService,
  FilingParametersService
} from '@inbound/services';
import {
  AddUserActionsPropertyToModelHandler,
  AddUserHighlightingTypePropertyToModelHandler,
  SameFilingHeaderHandler
} from '@inbound/handlers';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { LayoutService } from '@common/services/layout.service';
import { ToastrService } from 'ngx-toastr';
import { FileUploadService, ConfigurationService } from '@common/services';
import { AddManifestModalComponent } from '@inbound/add-manifest-modal/add-manifest-modal.component';
import { AvailableActions, HighlightingType, PageConfigNames } from '@common/models';

@Component({
  selector: 'lxft-inbound-bridge-list',
  templateUrl: 'list.component.html'
})
export class InboundBridgeRailListComponent extends CommonInboundList implements OnInit {

  private consolidatedFilingHeaderHandler = new SameFilingHeaderHandler(this.getRowId, this.getFilingHeaderId);

  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('htsTmpl')
  htsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  statusTmpl: TemplateRef<any>;

  protected get actionsTemplate(): TemplateRef<any> {
    return this.actionsTmpl;
  }
  protected get statusTemplate(): TemplateRef<any> {
    return this.statusTmpl;
  }
  get massUploadUrl(): string {
    return 'inbound/rail/documents-upload';
  }

  public validRecords: { valid: boolean; errorMessage: string } = {
    valid: true,
    errorMessage: ''
  };
  public pageAvailableActions: AvailableActions;
  public availableActions: InboundRecordListActions = new InboundRecordListActions();
  public infoIconType = IconType.Info;
  public isWorking: boolean = false;

  constructor(
    protected gridService: GridService,
    protected events: EventsService,
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridStorageService: GridStorageService,
    protected route: ActivatedRoute,
    protected router: Router,
    protected zone: NgZone,
    protected modal: ModalService,
    private configuration: InboundConfigurationService,
    protected inboundRecordsApiService: InboundRecordsApiService,
    protected inboundRecordsService: InboundRecordsService,
    protected singleFilingService: SingleFilingService,
    private validator: InboundRecordsValidator,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private filingParametersService: FilingParametersService,
    sanitizer: DomSanitizer,
    protected toastr: ToastrService,
    protected fileService: FileUploadService,
    private configurationService: ConfigurationService,
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
      new GridDataHandler<any>('addUserActionsHandler', AddUserActionsPropertyToModelHandler.handler)
    );
    this.dataHandlers.add(
      new GridDataHandler<any>('addUserHighlightingHandler', AddUserHighlightingTypePropertyToModelHandler.handler)
    );
    this.dataHandlers.add(
      new GridDataHandler<any>('consolidatedFilingHeader', this.consolidatedFilingHeaderHandler.handler)
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
      .getPageActions(PageConfigNames.RailViewPageActions)
      .subscribe(result => (this.pageAvailableActions = result));
  }

  updateGridColumns() {
    this.setFirstColumnBorder();
    this.setCheckboxColumns();
    this.setHTSColumn();
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

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    this.columns.push(this.hideColumnBorder(field));
  }

  setHTSColumn() {
    const htsField = this.columns.find(item => item.prop === 'HTS');
    htsField.cellTemplate = this.htsTmpl;
  }

  getIconTypeFor(row: any): IconType {
    const highlightingType = this.getRowHighlighting(row);
    const iconType = typeMappHighlightingIcon.find(m => m.highlightingType === highlightingType);
    return iconType ? iconType.iconType : IconType.None;
  }

  getRowHighlighting(row: any): HighlightingType {
    return row.UserHighlightingType && row.UserHighlightingType !== HighlightingType.NoHighlighting
      ? row.UserHighlightingType
      : row.HighlightingType;
  }

  getRowClass = row => this.getHighlightingStyleForRow(row);

  private getHighlightingStyleForRow(row): string {
    const highlightingType = this.getRowHighlighting(row);
    return styleMappHighlightingType[highlightingType] ? styleMappHighlightingType[highlightingType] : '';
  }

  onSelect(event: { selected: any[] }) {
    super.OnSelect(event);
    this.updateSelectActions(this.selectedRows);
    this.validateSelectedRows(this.selectedRows);
    this.refreshGrid();
  }

  updateSelectActions(selectedRows: { FilingHeaderId: any }[]) {
    super.updateSelectActions(selectedRows);
    this.consolidatedFilingHeaderHandler.clear();
    if (selectedRows.length > 0) {
      this.consolidatedFilingHeaderHandler.set(
        selectedRows.map(x => x.FilingHeaderId).filter((x, i, arr) => arr.indexOf(x) === i),
        selectedRows.map(x => this.getRowId(x))
      );
    }
  }

  validateSelectedRows(selectedRows: { Id: number }[]): void {
    if (selectedRows.length > 0) {
      {
        const ids = selectedRows.map(s => s.Id);
        this.validator.validateSelectedRecords(ids).subscribe(result => {
          if (result) {
            this.validRecords.valid = result.IsValid;
            this.validRecords.errorMessage = result.CommonError;
            this.availableActions = result.Actions;
            this.zone.run(() => {
              this.refreshGrid();
            });
          }
        });
      }
    } else {
      this.clearAvailableActions();
    }
  }

  clearAvailableActions() {
    this.availableActions = new InboundRecordListActions();
  }

  clearSelected() {
    super.clearSelected();
    this.consolidatedFilingHeaderHandler.clear();
    this.refreshGrid();
    this.clearAvailableActions();
  }

  restore(ids: (string | number)[] = []): void {
    if (!ids || !ids.length) {
      ids = this.selectedRows.map(x => this.getRowId(x));
    }
    this.modal
      .confirm({
        text: `Are you sure you want to restore this ${ids.length} record(s)?`
      })
      .then(confirmed => {
        if (confirmed) {
          this.inboundRecordsApiService.restoreRecords(ids).subscribe(() => {
            ids.forEach(x => this.removeSelectedRow(x));
            this.validateSelectedRows(this.selectedRows);
            this.getPage();
          });
        }
      });
  }

  showManifest(manifestId: any) {
    const data = {
      manifestId: manifestId,
      windowClass: 'modal-m'
    };
    this.modal.open(ManifestComponent, data).catch(() => {});
  }

  fileUnitTrainRecords(): void {
    const ids = this.selectedRows.map(r => r.Id);
    this.inboundRecordsApiService.startUnitTrainFiling(ids).subscribe(filingHeaderId => {
      this.goTo([filingHeaderId], ids);
    });
  }

  protected goTo(filingHeaderIds: number[], recordsIds: number[] = []): void {
    this.filingParametersService.clear();
    this.filingParametersService.filingHeaderIds = filingHeaderIds;
    this.router.navigate(['review-and-file'], {
      relativeTo: this.route
    });
  }

  view(): void {
    const filingHeaderIds: number[] = this.selectedRows.map(r => r.FilingHeaderId);
    this.inboundRecordsApiService.getRecordIds(filingHeaderIds).subscribe((ids: number[]) => {
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
    this.modal.open(AddManifestModalComponent, data).then(() => this.getPage()).catch(() => {});
}

/**
 * Edits selected row initial data
 * @param row selected row
 */
editInitial(row: any) {
  const data = {
    windowClass: 'modal-m',
    rowData: row,
  };
  this.modal.open(AddManifestModalComponent, data).then(() => this.getPage()).catch(() => {});
}
}
