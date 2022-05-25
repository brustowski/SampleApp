import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { OnInit, NgZone, TemplateRef, ChangeDetectorRef, ViewChild } from '@angular/core';
import {
  GridService,
  GridPageApiService,
  GridPageColumnsService,
  GridStorageService
} from '@common/grid/services';
import {
  InboundRecordsService,
  SingleFilingService,
  InboundRecordsApiService
} from '@inbound/services';
import { ActivatedRoute, Router } from '@angular/router';
import { GridDataHandler } from '@common/grid/handlers';
import { mappingStatusTypes, filingStatusTypes, jobStatusTypes } from '@app/utils';
import { ModalService, EventsService, FileUploadService } from '@common/services';
import { LayoutService } from '@common/services/layout.service';
import { SameStatusGroupHandler } from '@inbound/handlers/same-status-group-handler';
import { DomSanitizer } from '@angular/platform-browser';
import { FileUploadModalComponent } from '@common/file-uploader';
import { FileUploader, FileItem, ParsedResponseHeaders } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import * as R from 'ramda';
import { NgbTypeaheadWindow } from '@ng-bootstrap/ng-bootstrap/typeahead/typeahead-window';
import { typeWithParameters } from '@angular/compiler/src/render3/util';

export abstract class CommonInboundList extends GridPageCtrl implements OnInit {
  private sameStatusHandler = new SameStatusGroupHandler(
    this.getMappingStatus,
    this.getFilingStatus
  );

  @ViewChild('filingNumberTmpl')
  filingNumberTmpl: TemplateRef<any>;

  protected massUploader: FileUploader;

  public get importUrl(): string {
    return `${this.paginationOptions.pathForApi}/import`;
  }

  protected abstract get actionsTemplate(): TemplateRef<any>;
  protected abstract get statusTemplate(): TemplateRef<any>;

  constructor(
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected inboundRecordsService: InboundRecordsService,
    protected inboundRecordsApiService: InboundRecordsApiService,
    protected singleFilingService: SingleFilingService,
    protected zone: NgZone,
    protected route: ActivatedRoute,
    protected router: Router,
    protected modal: ModalService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private sanitizer: DomSanitizer,
    protected fileService: FileUploadService,
    protected toastr: ToastrService,
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);
  }

  ngOnInit() {
    this.autoHeight = true;
    super.ngOnInit();
    this.dataHandlers.add(
      new GridDataHandler<any>(
        'sameStatusGroupHandler',
        this.sameStatusHandler.handler,
        this.sameStatusHandler.checker
      )
    );
    this.initMassUploadFileUploader();
  }

  isNotNil(value: any): boolean {
    return !R.isNil(value);
  }

  getRowId(row: any): number | string {
    return row.Id;
  }

  getFilingHeaderId(row: any): number | string {
    return row.FilingHeaderId;
  }

  getFilingStatus(row: any): number | string {
    return row.FilingStatus;
  }

  getMappingStatus(row: any): number | string {
    return row.MappingStatus;
  }

  updateSelectActions(selectedRows: any[]): void {
    this.sameStatusHandler.clear();
    if (selectedRows.length > 0) {
      const row = selectedRows[0];
      this.sameStatusHandler.set(row.MappingStatus, row.FilingStatus);
    }
  }

  clearSelected() {
    super.clearSelected();
    this.sameStatusHandler.clear();
  }

  setCheckboxColumns() {
    const checkboxField = this.columnsService.setCheckboxColumn();
    this.columns.unshift(this.hideColumnBorder(checkboxField));
  }

  setFilingNumberColumn(): any {
    const jobNumberColumn = this.columns.find(item => item.prop === 'FilingNumber');
    jobNumberColumn.frozenRight = true;
    jobNumberColumn.cellTemplate = this.filingNumberTmpl;
  }

  createRecords(ids: (string | number)[] = []) {
    if (!ids || !ids.length) {
      ids = this.selectedRows.map(x => this.getRowId(x));
    }
    this.inboundRecordsApiService
      .startFiling(ids)
      .subscribe(filingHeaderIds => {
        if (filingHeaderIds && filingHeaderIds.length) {
          this.goTo(filingHeaderIds, ids);
        } else {
          this.toastr.error('Can\'t proceed due to in progress or validation failed record(s)');
        }
      });
  }

  edit(): void {
    const filingHeaderIds: number[] = this.selectedRows.map(
      r => r.FilingHeaderId
    );
    this.inboundRecordsApiService
      .canBeEdited(filingHeaderIds)
      .filter((isAllowed: boolean) => isAllowed)
      .switchMap(() =>
        this.inboundRecordsApiService.getRecordIds(filingHeaderIds)
      )
      .subscribe((ids: number[]) => {
        this.goTo(filingHeaderIds, ids);
      });
  }

  protected goTo(filingHeaderIds: number[], recordsIds: (string | number)[] = []): void {
    filingHeaderIds.length === 1
      ? this.goToReviewAndFile(filingHeaderIds[0], recordsIds)
      : this.goToReviewAndFileSingle(filingHeaderIds);
  }

  protected goToReviewAndFile(
    filingHeaderId: number,
    inboundRecordIds: (string | number)[] = []
  ) {
    this.inboundRecordsService.clear();
    this.inboundRecordsService.setIdentifiersForFiling(
      filingHeaderId,
      inboundRecordIds
    );
    this.zone.run(() => {
      this.router.navigate(['review-and-file'], { relativeTo: this.route });
    });
  }

  protected goToReviewAndFileSingle(filingHeaderIds: number[]): void {
    this.singleFilingService.clear();
    this.inboundRecordsService.clear();
    this.singleFilingService.filingHeaderIds = filingHeaderIds;
    this.router.navigate(['review-and-file-single'], {
      relativeTo: this.route
    });
  }

  view(): void {
    const filingHeaderIds: number[] = this.selectedRows.map(
      r => r.FilingHeaderId
    );
    this.inboundRecordsApiService
      .getRecordIds(filingHeaderIds)
      .subscribe((ids: number[]) =>
        ids.length === 1
          ? this.goToViewSingleRecord(filingHeaderIds[0])
          : this.goToViewMultipleRecords(filingHeaderIds)
      );
  }

  protected goToViewSingleRecord(filingHeaderId: number) {
    this.inboundRecordsService.clear();
    this.singleFilingService.clear();
    this.inboundRecordsService.setIdentifiersForFiling(filingHeaderId, []);
    this.zone.run(() => {
      this.router.navigate(['view'], { relativeTo: this.route });
    });
  }

  protected goToViewMultipleRecords(filingHeaderIds: number[]) {
    this.inboundRecordsService.clear();
    this.singleFilingService.clear();
    this.singleFilingService.filingHeaderIds = filingHeaderIds;
    this.zone.run(() => {
      this.router.navigate(['view-single'], { relativeTo: this.route });
    });
  }

  getMappingLabelType(mappingStatus): string {
    return this.getStatusTypeFromList(mappingStatus, mappingStatusTypes);
  }

  getJobLabelType(jobStatus): string {
    return this.getStatusTypeFromList(jobStatus, jobStatusTypes);
  }

  getFilingLabelType(filingStatus): string {
    return this.getStatusTypeFromList(filingStatus, filingStatusTypes);
  }

  getStatusTypeFromList(
    status: string | number,
    typesList: { [x: string]: string }
  ): string {
    return typesList[status];
  }

  protected setActionsGridColumn() {
    const actionCol = this.columnsService.getActionColumn(this.actionsTemplate);
    this.columns.push(actionCol);
  }

  protected setStatusColumns() {
    const statusCol = this.columnsService.getStatusColumn(this.statusTemplate);
    this.columns.push(statusCol);
  }

  protected delete(ids: (string | number)[] = []) {
    if (!ids || !ids.length) {
      ids = this.selectedRows.map(x => this.getRowId(x));
    }
    if (!ids.length) { return; }

    let message =
      ids.length > 1
        ? `Are you sure you want to delete selected records?`
        : 'Are you sure you want to delete record?';

    if (this.selectedRows.find(x => x.Actions && x.Actions.Delete === false)) {
      message = 'Records already created in CW will not be deleted. Do you want to proceed with other record(s)?';
    }

    this.modal
      .confirm({
        text: message
      })
      .then(confirmed => {
        if (confirmed) {
          this.inboundRecordsApiService.deleteRecords(ids).subscribe(() => {
            ids.forEach(x => this.removeSelectedRow(x));
            this.validateSelectedRows(this.selectedRows);
            this.getPage();
          });
        }
      });
  }

  protected removeSelectedRow(id: string | number): void {
    const record = this.selectedRows.find(item => item.Id === id);
    this.selectedRows.splice(this.selectedRows.indexOf(record), 1);
  }

  abstract validateSelectedRows(rows: any[]): void;

  protected OnSelect(rows: { selected: any[] }): void {
    this.selectedRows = rows.selected;
  }

  sanitize(url: string) {
    return this.sanitizer.bypassSecurityTrustUrl(url);
  }

  exportList() {
    this.api.exportToExcel(this.paginationOptions.gridName, this.getPageParams());
  }

  abstract get massUploadUrl(): string;

  private initMassUploadFileUploader() {
    this.massUploader = this.fileService.createFileUploader(this.massUploadUrl);
    this.massUploader.onErrorItem = (item: FileItem, response: string) => {
      const responseJson = JSON.parse(response);
      this.toastr.error(`Error while uploading ${item.file.name}: ${responseJson.Message}`);
      item.isUploaded = false;
    };
    this.massUploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
      form.append('recordIds', this.selectedRows.map(x => this.getRowId(x)));
      form.append('docType', fileItem.formData['docType'] || '');
      form.append('description', fileItem.formData['description'] || '');
    };
  }

  uploadDocuments() {
    this.modal.open(FileUploadModalComponent, { uploader: this.massUploader }).then(
      () => this.massUploader.clearQueue(),
      () => this.massUploader.clearQueue());
  }
}
