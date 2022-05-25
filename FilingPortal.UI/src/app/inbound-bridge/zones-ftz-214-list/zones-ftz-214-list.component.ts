import {
  Component,
  OnInit,
  ViewChild,
  TemplateRef,
  NgZone,
  ChangeDetectorRef,
  ElementRef
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as R from 'ramda';

import { InboundRecordListActions, InboundRecordDocument } from '@inbound/models';

import {
  EventsService,
  ModalService,
  ConfigurationService,
  FileUploadService,
  AuthenticationService
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
  FilingParametersService,
  PreFilingSevice
} from '@inbound/services';

import { AddUserActionsPropertyToModelHandler } from '@inbound/handlers';

import {
  typeMappHighlightingIcon,
  styleMappHighlightingType,
} from '@app/utils';
import { IconType } from '@common/icon-tooltip';
import { AvailableActions, HighlightingType, PreFilingValidationErrorType } from '@common/models';
import { ZonesFtz214PageConfigNames } from '@common/models/zones-ftz214-models';
import { GridDataHandler } from '@common/grid/handlers';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { LayoutService } from '@common/services/layout.service';
import { FileUploadDetailedResultComponent } from '@common/file-uploader';
import { ToastrService } from 'ngx-toastr';
import { FileUploader } from 'ng2-file-upload';
import { DomSanitizer } from '@angular/platform-browser';
import { DocDownloadService } from '@inbound/services/doc-download.service';
import { FileUploadDetailedResultModel } from '@common/models/file-upload-detailed-result-model';
import { from } from 'rxjs';
import { FiltersPanelComponent } from '@common/filters/filters-panel';
import { AddAvailableApplicantsHandler } from '@inbound/handlers/add-available-applicants-to-model-handler';
import { AddAvailableFtzOperatorsHandler } from '@inbound/handlers/add-available-ftzoperators-to-model-handler';
import { TruckExportFilingConfirmationDialogComponent } from '@inbound/truck-export-filing-confirmation-dialog';

@Component({
  selector: 'lxft-zones-ftz-214-list',
  templateUrl: './zones-ftz-214-list.component.html'
})
export class ZonesFtz214ListComponent extends CommonInboundList implements OnInit {
  get massUploadUrl(): string {
    return 'inbond/documents-upload';
  }
  public get uploadUrl(): string {
    return `${this.paginationOptions.pathForApi}/upload`;
  }

  @ViewChild('fileInput')
  public fileInput: ElementRef;
  @ViewChild('notificationIconTmpl')
  public notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  public statusTmpl: TemplateRef<any>;
  @ViewChild(FiltersPanelComponent)
  private filterPanel: FiltersPanelComponent;
  @ViewChild('applicantSelectionTemplate')
  public applicantSelectionTemplate: TemplateRef<any>;
  @ViewChild('ftzOperatorSelectionTemplate')
  public ftzOperatoSelectionTemplate: TemplateRef<any>;

  protected get actionsTemplate(): TemplateRef<any> {
    return this.actionsTmpl;
  }
  protected get statusTemplate(): TemplateRef<any> {
    return this.statusTmpl;
  }
  public get importXmlUrl(): string {
    return `${this.paginationOptions.pathForApi}/upload-xml`;
  }

  public pageAvailableActions: AvailableActions;
  public availableActions = new InboundRecordListActions();
  public isWorking: boolean = false;
  public uploader: FileUploader;

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
    private docDownloadService: DocDownloadService,
    private authenticationService: AuthenticationService,
    private preFilingSevice: PreFilingSevice
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
    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addAvailableApplicantsHandler',
        AddAvailableApplicantsHandler.handler
      )
    );
    
    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addAvailableFtzOperatorsHandler',
        AddAvailableFtzOperatorsHandler.handler
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
      .getPageActions(ZonesFtz214PageConfigNames.InboundViewPageActions)
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
    this.setApplicantColumnTemplate();
    this.setFtzOperatorColumnTemplate();
  }

  setApplicantColumnTemplate() {
    const column = this.columns.find(x => x.prop === 'Applicant');
    column.cellTemplate = this.applicantSelectionTemplate;
  }

  setFtzOperatorColumnTemplate() {
    const column = this.columns.find(x => x.prop === 'FtzOperator');
    column.cellTemplate = this.ftzOperatoSelectionTemplate;
  }

  protected setActionsGridColumn() {
    const actionCol = this.columnsService.getActionColumn(this.actionsTemplate);
    actionCol.width = 141;
    actionCol.minWidth = 141;
    actionCol.maxWidth = 141;
    this.columns.push(actionCol);
  }

  private setFirstColumnBorder() {
    if (this.columns && this.columns.length) {
      this.columns[0] = this.hideColumnBorder(this.columns[0]);
    }
  }

  protected setStatusColumns() {
    const statusCol = this.columnsService.getStatusColumn(this.statusTemplate);
    statusCol.name = 'Job Status';
    this.columns.push(statusCol);
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

  downloadXml(row: any): void {
    const document = <InboundRecordDocument>{ id: this.getRowId(row), name: 'inbound_xml' };
    this.docDownloadService.processDocument(document);
  }

  validateRecords() {
    const pageParams = this.getPageParams();
    this.inboundRecordsApiService.validate(pageParams.FilterSettings).subscribe(() => {
      this.getPage();
      this.toastr.success('Validation completed');
    });
  }

  getStatusTypeFromList(
    status: string | number,
    typesList: { [x: string]: string }
  ): string {
    return `${super.getStatusTypeFromList(status, typesList)} no-before`;
  }

  saveSelectedImporter(row: any, value: string) {
    const id = this.getRowId(row);
    this.inboundRecordsApiService.setImporter(id, value).subscribe(
      () => {
        this.validateRecords();
      }
    );
  }

  saveSelectedFtzOperator(row: any, value: string) {
    const id = this.getRowId(row);
    this.inboundRecordsApiService.setFtzoperator(id, value).subscribe(
      () => {
        this.validateRecords();
      }
    );
  }

  createRecords(ids: number[]): void {
    const selectedIds = ids && ids.length ? ids : <number[]>this.selectedRows.map(x => this.getRowId(x));
    const idsNumber = selectedIds.length;
    this.preFilingSevice.validateRecords(selectedIds).subscribe((result) => {
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
}
