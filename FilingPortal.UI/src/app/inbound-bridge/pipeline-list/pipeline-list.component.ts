import {
  Component,
  OnInit,
  TemplateRef,
  ViewChild,
  NgZone,
  ElementRef,
  ChangeDetectorRef
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/finally';

import * as R from 'ramda';

import { GridService } from '@common/grid/services/grid.service';
import { EventsService } from '@common/services/events.service';
import { GridPageApiService } from '@common/grid/services/grid-page-api.service';
import { GridPageColumnsService } from '@common/grid/services/grid-page-columns.service';
import { ModalService } from '@common/services/modal.service';

import {
  typeMappHighlightingIcon,
  styleMappHighlightingType
} from '@app/utils';

import {
  InboundRecordsValidator,
  InboundRecordsApiService,
  InboundRecordsService,
  InboundConfigurationService,
  SingleFilingService,
  FilingParametersService
} from '@inbound/services';
import { GridDataHandler } from '@common/grid/handlers';
import { IconType } from '@common/icon-tooltip';
import { GridStorageService } from '@common/grid/services/grid-storage.service';
import { FileUploader } from 'ng2-file-upload';
import { FileUploadService, ConfigurationService } from '@common/services';
import { FileUploadResultComponent } from '@common/file-uploader';
import {
  AddUserActionsPropertyToModelHandler,
  AddUserHighlightingTypePropertyToModelHandler,
  InboundRecordValidationResultHandler} from '@inbound/handlers';
import { AvailableActions, HighlightingType, PageConfigNames } from '@common/models';
import {
  InboundRecordListActions,
  InboundRecordValidationResultViewModel
} from '@inbound/models';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { ToastrService } from 'ngx-toastr';
import { LayoutService } from '@common/services/layout.service';
import { DomSanitizer } from '@angular/platform-browser';
import { PipelineMassUploadComponent } from '@inbound/pipeline-mass-upload/pipeline-mass-upload.component';

@Component({
  selector: 'lxft-pipeline-list',
  templateUrl: 'pipeline-list.component.html'
})
export class PipelineListComponent extends CommonInboundList implements OnInit {
  get massUploadUrl(): string {
    return 'inbound/pipeline/documents-upload';
  }
  public get uploadUrl(): string {
    return `${this.paginationOptions.pathForApi}/upload`; // 'inbound/pipeline/upload'
  }

  @ViewChild('fileInput')
  public fileInput: ElementRef;
  @ViewChild('notificationIconTmpl')
  public notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  public statusTmpl: TemplateRef<any>;

  protected get actionsTemplate(): TemplateRef<any> { return this.actionsTmpl; }
  protected get statusTemplate(): TemplateRef<any> { return this.statusTmpl; }

  public uploader: FileUploader;
  public pageAvailableActions: AvailableActions;
  public availableActions = new InboundRecordListActions();
  private resultHandler = new InboundRecordValidationResultHandler();
  public isValid: boolean;

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
    private configurationService: ConfigurationService,
    protected inboundRecordsApiService: InboundRecordsApiService,
    protected inboundRecordsService: InboundRecordsService,
    protected singleFilingService: SingleFilingService,
    private validator: InboundRecordsValidator,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private filingParametersService: FilingParametersService,
    sanitizer: DomSanitizer,
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
      eventsService,
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

    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addUserHighlightingHandler',
        AddUserHighlightingTypePropertyToModelHandler.handler
      )
    );
    this.dataHandlers.add(
      new GridDataHandler<any>(
        'validationResultHandler',
        this.resultHandler.handler
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
      .getPageActions(PageConfigNames.PipelineViewPageActions)
      .subscribe(result => (this.pageAvailableActions = result));
  }

  onUploadSuccess($event: any): void {
    this.getPage();
    const result = this.fileService.parseFileUploadResultResponse($event);
    this.modal.open(FileUploadResultComponent, result);
  }

  onUploadError($event: any): void {
    this.toastr.error($event);
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
    return row.UserHighlightingType &&
      row.UserHighlightingType !== HighlightingType.NoHighlighting
      ? row.UserHighlightingType
      : row.HighlightingType;
  }

  getRowClass = (row: any) => this.getHighlightingStyleForRow(row);

  private getHighlightingStyleForRow(row: any): string {
    const highlightingType = this.getRowHighlighting(row);
    return styleMappHighlightingType[highlightingType]
      ? styleMappHighlightingType[highlightingType]
      : '';
  }

  onSelect(event: { selected: any[] }) {
    super.OnSelect(event);
    this.updateSelectActions(this.selectedRows);
    this.validateSelectedRows(this.selectedRows);
    this.refreshGrid();
  }

  validateSelectedRows(selectedRows: any[]): void {
    if (selectedRows.length > 0) {
      const ids = selectedRows.map((s: { Id: number }) => s.Id);
      this.validator
        .validateSelectedRecords(ids)
        .subscribe((result: InboundRecordValidationResultViewModel) => {
          if (result) {
            this.isValid = result.IsValid;
            this.resultHandler.validationResult = result;
            this.availableActions = result.Actions;
            this.zone.run(() => {
              this.refreshGrid();
            });
          }
        });
    } else {
      this.resultHandler.clear();
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
  }

  exportList() {
    this.api.exportToExcel(
      this.paginationOptions.gridName,
      this.getPageParams()
    );
  }

  downloadTemplate(): void {
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
      .subscribe((ids: number[]) => {
        this.filingParametersService.clear();
        this.filingParametersService.filingHeaderIds = filingHeaderIds;
        this.router.navigate(['view'], {
          relativeTo: this.route
        });
      });
  }

  uploadDocuments() {
    this.modal.open(PipelineMassUploadComponent, {
      uploader: this.massUploader,
      batchCodes: this.selectedRows.map(x => x.Batch)
    }).then(
      () => this.massUploader.clearQueue(),
      () => this.massUploader.clearQueue());
  }
}
