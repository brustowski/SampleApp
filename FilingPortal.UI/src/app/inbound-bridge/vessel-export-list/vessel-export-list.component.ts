import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  TemplateRef,
  NgZone,
  ChangeDetectorRef
} from '@angular/core';
import { CommonInboundList } from '@inbound/common-inbound-list';
import { AvailableActions, PageConfigNames, HighlightingType } from '@common/models';
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
  ConfigurationService
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
import { FileUploadResultComponent } from '@common/file-uploader';
import {
  typeMappHighlightingIcon,
  styleMappHighlightingType
} from '@app/utils';
import * as R from 'ramda';
import { AddVesselModalComponent } from '@inbound/add-vessel-modal';
import { EditVesselModalComponent } from '@inbound/edit-vessel-modal';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'lxft-vessel-export-list',
  templateUrl: './vessel-export-list.component.html'
})
export class VesselExportListComponent extends CommonInboundList
  implements OnInit {

  @ViewChild('fileInput')
  public fileInput: ElementRef;
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
  get massUploadUrl(): string {
    return 'export/vessel/documents-upload';
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
    this.initFileUploader();
    this.setPageAvailableActions();
  }

  setPageAvailableActions(): void {
    this.configurationService
      .getPageActions(PageConfigNames.VesselExportViewPageActions)
      .subscribe(result => (this.pageAvailableActions = result));
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
        this.modal.open(FileUploadResultComponent, result);
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
      .subscribe((ids: number[]) => {
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
      isExport: true
    };
    this.modal.open(AddVesselModalComponent, data).then(() => this.getPage()).catch(() => {});
}

/**
 * Edits selected row initial data
 * @param row selected row
 */
editInitial(row: any) {
  const data = {
    windowClass: 'modal-m',
    rowData: row,
    isExport: true
  };
  this.modal.open(EditVesselModalComponent, data).then(() => this.getPage()).catch(() => {});
}
}
