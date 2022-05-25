import {
  Component,
  OnInit,
  ViewChild,
  TemplateRef,
  NgZone,
  ChangeDetectorRef
} from '@angular/core';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { AvailableActions, PageConfigNames, HighlightingType, PreFilingValidationErrorType } from '@common/models';
import { InboundRecordListActions } from '@inbound/models';
import { FileUploader } from 'ng2-file-upload';
import { IconType } from '@common/icon-tooltip';
import {
  GridService,
  GridPageApiService,
  GridPageColumnsService,
  GridStorageService
} from '@common/grid/services';
import {
  EventsService,
  FileUploadService,
  ModalService,
  ConfigurationService,
  AuthenticationService
} from '@common/services';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import {
  InboundConfigurationService,
  InboundRecordsApiService,
  InboundRecordsService,
  SingleFilingService,
  FilingParametersService
} from '@inbound/services';
import { LayoutService } from '@common/services/layout.service';
import { GridDataHandler } from '@common/grid/handlers';
import { AddUserActionsPropertyToModelHandler } from '@inbound/handlers';
import { FileUploadDetailedResultComponent } from '@common/file-uploader';
import {
  typeMappHighlightingIcon,
  styleMappHighlightingType, updateStatusTypes
} from '@app/utils';
import * as R from 'ramda';
import { DomSanitizer } from '@angular/platform-browser';
import { PageParams } from '@common/grid/models';
import { FiltersPanelComponent } from '@common/filters/filters-panel';
import { from } from 'rxjs';
import { FileUploadDetailedResultModel } from '@common/models/file-upload-detailed-result-model';
import { PreFilingSevice } from '@inbound/services';
import { TruckExportFilingConfirmationDialogComponent } from '@inbound/truck-export-filing-confirmation-dialog';

@Component({
  selector: 'lxft-truck-export-list',
  templateUrl: './truck-export-list.component.html'
})
export class TruckExportListComponent extends CommonInboundList
  implements OnInit {

  get massUploadUrl(): string {
    return 'export/truck/documents-upload';
  }

  @ViewChild('notificationIconTmpl')
  public notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  public statusTmpl: TemplateRef<any>;
  @ViewChild(FiltersPanelComponent)
  private filterPanel: FiltersPanelComponent;

  protected get actionsTemplate(): TemplateRef<any> {
    return this.actionsTmpl;
  }
  protected get statusTemplate(): TemplateRef<any> {
    return this.statusTmpl;
  }
  public pageAvailableActions: AvailableActions;
  public availableActions = new InboundRecordListActions();
  public isWorking: boolean = false;
  public uploader: FileUploader;
  currentUser: string;
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
    sanitizer: DomSanitizer,
    private authenticationService: AuthenticationService,
    private truckExportService: PreFilingSevice
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
    this.authenticationService.getUser().subscribe(user => this.currentUser = user.UserAccount);
  }

  setPageAvailableActions(): void {
    this.configurationService
      .getPageActions(PageConfigNames.TruckExportViewPageActions)
      .subscribe(result => (this.pageAvailableActions = result));
  }

  onUploadSuccess($event: any): void {
    const result = FileUploadDetailedResultModel.Parse($event);
    from(this.modal.open(FileUploadDetailedResultComponent,
      { uploadResult: result, windowClass: 'message-box' }))
      .switchMap(() => this.authenticationService.getUser())
      .subscribe(user => {
        this.clearSelected();
        this.filterPanel.resetFilters(true);

        this.filterPanel.setFilterByFieldName('ModifiedUser', user.UserAccount);
        this.filterPanel.setFilterByFieldName('ModifiedDate', new Date().toISOString());
        this.filterPanel.setFilters();
      });
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

  protected setStatusColumns() {
    const statusCol = this.columnsService.getStatusColumn(this.statusTemplate);
    statusCol.name = 'Job Status';
    this.columns.push(statusCol);
  }

  getUpdateLabelType(status): string {
    return this.getStatusTypeFromList(status, updateStatusTypes);
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
    this.refreshGrid();
  }

  validateSelectedRows(selectedRows: any[]): void {
    // Validation is not required, all actions available
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

  getPageParams(): PageParams {
    const pageParams = this.gridService.getPageParams(this.paginationOptions, this.sortOptions, this.filtersOptions);
    return pageParams;
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

  view(filingHeaderIds: number[] = []) {
    if (!filingHeaderIds || !filingHeaderIds.length) {
      filingHeaderIds = this.selectedRows.map(r => r.FilingHeaderId);
    }
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

  validateRecords() {
    const pageParams = this.getPageParams();
    this.inboundRecordsApiService.validate(pageParams.FilterSettings).subscribe(() => {
      this.getPage();
      this.toastr.success('Validation completed');
    });
  }

  getJobNumber() {
    const message = `Job Numbers will be retrieved for ${this.selectedRows.length} selected records. Continue?`;
    this.modal.confirm({ text: message, cancelButtonTitle: 'No', okButtonTitle: 'Yes' }).then(confirmed => {
      if (confirmed) {
        this.inboundRecordsApiService.getJobNumber(this.selectedRows.map(x => this.getRowId(x))).subscribe(processedRecords => {
          this.toastr.success(`Job Numbers updated for ${processedRecords} records.`);
          this.getPage();
        });
      }
    });
  }

  createRecords(ids: number[]): void {
    const selectedIds = ids && ids.length ? ids : <number[]>this.selectedRows.map(x => this.getRowId(x));
    const idsNumber = selectedIds.length;
    this.truckExportService.validateRecords(selectedIds).subscribe((result) => {
      const missingJobNumberReocrdsCount = result.filter(x => x.ErrorType === PreFilingValidationErrorType.MissingJobNumber).length;
      const validationFailedRecordsCount = result.filter(x => x.ErrorType === PreFilingValidationErrorType.ValidationFailed).length;
      const inProgressRecordsCount = result.filter(x => x.ErrorType === PreFilingValidationErrorType.InvalidStatus).length;
      const validRecordIds = result.filter(x => x.ErrorType === PreFilingValidationErrorType.None).map(x => x.Id);
      if (idsNumber === validRecordIds.length) {
        super.createRecords(validRecordIds);
      } else {
        const message: string[] = [];
        if (missingJobNumberReocrdsCount) {
          message.push(`Update requires Job Number for ${missingJobNumberReocrdsCount} records.`);
        }
        if (validationFailedRecordsCount) {
          message.push(`Validation failed for ${validationFailedRecordsCount} records.`);
        }
        if (inProgressRecordsCount) {
          message.push(`In-Progress Job Status is set for ${validationFailedRecordsCount} records.`);
        }
        message.push(`${validRecordIds.length} records will be processed.`);
        this.modal.open(TruckExportFilingConfirmationDialogComponent, { text: message.join('<br/>'), okButtonTitle: 'Continue' }, true)
          .then(() => super.createRecords(validRecordIds));
      }
    });
  }

  getStatusTypeFromList(
    status: string | number,
    typesList: { [x: string]: string }
  ): string {
    return `${super.getStatusTypeFromList(status, typesList)} no-before`;
  }
}
