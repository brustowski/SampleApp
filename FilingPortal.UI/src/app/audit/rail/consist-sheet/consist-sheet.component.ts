import { Component, OnInit, ChangeDetectorRef, Inject, ViewChild, ElementRef, TemplateRef } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { ActivatedRoute } from '@angular/router';
import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '@common/grid/services';
import { ModalService, EventsService, LayoutService, FileUploadService, ConfigurationService } from '@common/services';
import { ToastrService } from 'ngx-toastr';
import { FileUploader } from 'ng2-file-upload';
import { IGridsConfigurationService, GridWithExcelTemplate } from '@common/interfaces';
import { FileUploadResultComponent } from '@common/file-uploader';
import { AuditApiService } from '@app/audit/services/audit-api.service';
import { styleMappHighlightingType } from '@app/utils';
import { GridDataHandler } from '@common/grid/handlers';
import { AddUserHighlightingTypePropertyToModelHandler } from '@inbound/handlers';
import { IconType } from '@common/icon-tooltip';
import { HighlightingType } from '@common/models';

@Component({
  selector: 'lxft-consist-sheet',
  templateUrl: './consist-sheet.component.html'
})
export class ConsistSheetComponent extends GridPageCtrl implements OnInit, GridWithExcelTemplate {

  @ViewChild('fileInput')
  public fileInput: ElementRef<HTMLInputElement>;
  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;

  public uploader: FileUploader;
  public isWorking: boolean;

  constructor(protected route: ActivatedRoute,
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected modalService: ModalService,
    protected toastr: ToastrService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    @Inject('GridsConfigurationService') private configurationService: IGridsConfigurationService,
    private pageActionsService: ConfigurationService,
    private fileService: FileUploadService,
    private auditApiService: AuditApiService,
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);

    this.dataHandlers.add(
      new GridDataHandler<any>('addUserHighlightingHandler', AddUserHighlightingTypePropertyToModelHandler.handler)
    );
  }

  ngOnInit() {
    super.ngOnInit();
    this.autoHeight = true;
    this.loadInitialParameters();
    this.initFileUploader();
  }

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.configurationService.getGridConfig(name);
    return !!this.paginationOptions;
  }

  setPageAvailableActions(name: string): void {
    const pageName = this.configurationService.getPageActionsConfig(name);
    if (pageName) {
      this.pageActionsService.getPageActions(pageName).subscribe(result => (this.availableActions = result));
    }
  }

  protected loadInitialParameters(): void {
    const pageName: string = this.route.snapshot.data.name;
    if (pageName) {
      if (this.setPaginationOptions(pageName)) {
        this.initGridConfigAndData();
      }
      this.setPageAvailableActions(pageName);
    }
  }

  // todo: create separate file uploader component
  private initFileUploader() {
    this.uploader = this.fileService.createFileUploader(
      `${this.paginationOptions.pathForApi}/upload`
    );
    this.uploader.onBeforeUploadItem = () => {
      if (!this.isWorking) {
        this.isWorking = true;
      }
    };

    this.uploader.onCompleteAll = () => {
      this.isWorking = false;
      this.fileInput.nativeElement.value = '';
    };

    this.uploader.onAfterAddingFile = file => {
      file.upload();
      file.onSuccess = (response: string) => {
        this.getPage();
        const result = this.fileService.parseFileUploadResultResponse(response);
        this.modalService.open(FileUploadResultComponent, result);
      };
      file.onError = (response: string, status: number) => {
        let message = 'Unexpected error occurred during import process';
        if (status === 400 || status === 403) {
          const jsonResponse = JSON.parse(response);
          message = jsonResponse.Message;
        }
        this.toastr.error(message);
      };
    };
  }

  downloadTemplate(): void {
    this.api.downloadTemplate(this.paginationOptions.gridName);
  }

  verify() {
    this.auditApiService.verify().subscribe(result => {
      this.getTotalMatches();
      this.getPage();
    });
  }

  clear() {
    this.auditApiService.clear().subscribe(result => {
      this.setFilters();
    });
  }

  getRowClass = row => this.getHighlightingStyleForRow(row);

  private getHighlightingStyleForRow(row): string {
    const highlightingType = this.getRowHighlighting(row);
    return styleMappHighlightingType[highlightingType] ? styleMappHighlightingType[highlightingType] : '';
  }

  getRowHighlighting(row: any): HighlightingType {
    return row.UserHighlightingType && row.UserHighlightingType !== HighlightingType.NoHighlighting
      ? row.UserHighlightingType
      : row.HighlightingType;
  }

  updateGridColumns() {
    this.setFirstColumnBorder();
    this.setNotificationColumn();
  }

  private setFirstColumnBorder() {
    if (this.columns && this.columns.length) {
      this.columns[0] = this.hideColumnBorder(this.columns[0]);
    }
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    field.maxWidth = 27;
    this.columns.push(this.hideColumnBorder(field));
  }

  getIconTypeFor(row: any): IconType {
    const highlightingType = this.getRowHighlighting(row);
    if (highlightingType === HighlightingType.Error) {
      return IconType.Error;
    }
    return IconType.None;
  }
}
