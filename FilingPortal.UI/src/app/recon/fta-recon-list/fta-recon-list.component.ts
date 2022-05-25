import { Component, OnInit, ChangeDetectorRef, Inject, ViewChild, TemplateRef, NgZone, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { FileUploadResultComponent } from '@common/file-uploader';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { FiltersPanelComponent } from '@common/filters/filters-panel';

import { IGridsConfigurationService } from '@common/interfaces';
import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '@common/grid/services';
import { ModalService, EventsService, LayoutService, FileUploadService, ConfigurationService, AuthenticationService } from '@common/services';
import { SingleFilingService, InboundRecordsApiService, InboundRecordsService } from '@inbound/services';
import { ReconApiService } from '../services/recon-api.service';
import { ReconService } from '../services/recon.service';

import { GridDataHandler } from '@common/grid/handlers';
import { AddUserHighlightingTypePropertyToModelHandler } from '@inbound/handlers';

import { Column, ColumnConfiguration } from '@common/grid/models';
import { InboundRecordListActions } from '@inbound/models';
import { AvailableActions, HighlightingType, ReconGridNames } from '@common/models';
import { IconType } from '@common/icon-tooltip';

import { ftaReconStatusTypes, styleMappHighlightingType, typeMappHighlightingIcon } from '@app/utils';
import { from } from 'rxjs';

@Component({
  selector: 'lxft-fta-recon-list',
  templateUrl: './fta-recon-list.component.html'
})
export class FtaReconListComponent extends GridPageCtrl implements OnInit, OnDestroy {

  @ViewChild(FiltersPanelComponent)
  private filterPanel: FiltersPanelComponent;
  @ViewChild('columnTemplate')
  columnTemplate: TemplateRef<any>;
  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('statusTmpl')
  public statusTmpl: TemplateRef<any>;
  public pageAvailableActions: AvailableActions = {};

  public isWorking: boolean;
  public uploadUrl: string;

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
    protected fileService: FileUploadService,
    protected toastr: ToastrService,
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private pageActionsService: ConfigurationService,
    private reconApiService: ReconApiService,
    private reconService: ReconService,
    private authenticationService: AuthenticationService,
  ) {
    super(api,
      columnsService,
      gridService,
      gridStorageService,
      eventsService,
      cdr, layoutService
    );

    this.dataHandlers.add(
      new GridDataHandler<any>('addUserHighlightingHandler', AddUserHighlightingTypePropertyToModelHandler.handler)
    );
  }

  ngOnInit() {
    super.ngOnInit();
    this.autoHeight = true;
    this.loadInitialParameters();
  }

  ngOnDestroy() {
    this.reconService.setActions({});
  }

  validateSelectedRows(selectedRows: { Id: number }[]): void {
    if (selectedRows.length > 0) {
      const ids = selectedRows.map(s => s.Id);
      this.reconApiService
        .getAvailableActions(ids)
        .subscribe(result => {
          this.availableActions = result;
        });
    } else {
      this.clearAvailableActions();
    }
  }

  clearAvailableActions() {
    this.availableActions = new InboundRecordListActions();
  }

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.configurationService.getGridConfig(name);
    return !!this.paginationOptions;
  }

  setPageAvailableActions(name: string): void {
    const pageName = this.configurationService.getPageActionsConfig(name);
    if (pageName) {
      this.pageActionsService.getPageActions(pageName).subscribe(result => {
        this.pageAvailableActions = result;
        this.reconService.setActions(result);
      });
    }
  }

  protected loadInitialParameters(): void {
    const pageName: string = this.route.snapshot.data.name;
    if (pageName) {
      if (this.setPaginationOptions(pageName)) {
        this.initGridConfigAndData();
        this.uploadUrl = `imports/upload/grids/${ReconGridNames.FtaRecords}`;
      }
      this.setPageAvailableActions(pageName);
    }
  }

  downloadTemplate(): void {
    this.api.downloadTemplate(this.paginationOptions.gridName);
  }

  updateGridColumns() {
    this.columns.forEach(x => x.cellTemplate = this.columnTemplate);
    this.setFirstColumnBorder();
    this.setNotificationColumn();
    this.setCheckboxColumns();
    this.setStatusColumns();
    this.setActionsGridColumn();
  }

  private setFirstColumnBorder() {
    if (this.columns && this.columns.length) {
      this.columns[0] = this.hideColumnBorder(this.columns[0]);
    }
  }

  protected setActionsGridColumn() {
    const actionCol = this.columnsService.getActionColumn(this.actionsTmpl);
    this.columns.push(actionCol);
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    this.columns.push(this.hideColumnBorder(field));
  }

  setCheckboxColumns() {
    const checkboxField = this.columnsService.setCheckboxColumn();
    this.columns.unshift(this.hideColumnBorder(checkboxField));
  }

  protected setStatusColumns() {
    const statusCol = this.columnsService.getStatusColumn(this.statusTmpl);
    statusCol.name = 'FTA Status';
    this.columns.push(statusCol);
  }

  getRowHighlighting(row: any): HighlightingType {
    return row.UserHighlightingType && row.UserHighlightingType !== HighlightingType.NoHighlighting
      ? row.UserHighlightingType
      : row.HighlightingType;
  }

  getStatusType(status: string | number): string {
    return `${ftaReconStatusTypes[status]} no-before`;
  }

  getRowClass = row => this.getHighlightingStyleForRow(row);

  private getHighlightingStyleForRow(row): string {
    const highlightingType = this.getRowHighlighting(row);
    return styleMappHighlightingType[highlightingType] ? styleMappHighlightingType[highlightingType] : '';
  }

  getIconTypeFor(row: any): IconType {
    const highlightingType = this.getRowHighlighting(row);
    const iconType = typeMappHighlightingIcon.find(m => m.highlightingType === highlightingType);
    return iconType ? iconType.iconType : IconType.None;
  }

  getRowId(row: any): number | string {
    return row.Id;
  }

  protected removeSelectedRow(id: string | number): void {
    const record = this.selectedRows.find(item => item.Id === id);
    this.selectedRows.splice(this.selectedRows.indexOf(record), 1);
  }

  canBeChecked(row: any): boolean { return true; }

  onSelect(rows: { selected: any[] }) {
    this.selectedRows = rows.selected;
    this.validateSelectedRows(this.selectedRows);
    this.refreshGrid();
  }

  onUploadSuccess($event: any): void {
    const result = this.fileService.parseFileUploadResultResponse($event);
    from(this.modal.open(FileUploadResultComponent, result))
      .switchMap(() => this.authenticationService.getUser())
      .subscribe(user => {
        this.filterPanel.resetFilters(true);
        this.filterPanel.setFilterByFieldName('CreatedUser', user.UserAccount);
        this.filterPanel.setFilterByFieldName('CreatedDate', new Date().toISOString());
        this.filterPanel.setFilters();
      });
  }

  onUploadError($event: any): void {
    this.toastr.error($event);
  }

  process(id: number | string): void {
    const selecteId = id ? [id] : this.selectedRows.map(this.getRowId);
    if (selecteId.length) {
      const message = `Have you updated FTA Recon filing in CW for ${selecteId.length} of records selected?`;
      this.modal.confirm({ text: message, cancelButtonTitle: 'No', okButtonTitle: 'Yes' }).then(confirmed => {
        if (confirmed) {
          this.reconApiService.process(selecteId).subscribe(() => {
            this.toastr.success(`Selected records were processed successfully`);
            this.getPage();
          });
        }
      });
    }
  }

  updateColumnSettings(settings: ColumnConfiguration[]): void {
    settings.forEach(x => {
      const column = this.columns.find(y => y.prop === x.prop);
      if (column) {
        column.isVisible = x.isVisible;
      }
    });
    this.columns = [... this.columns];
    this.gridStorageService.setColumnConfiguration(this.paginationOptions.gridName, settings);
    this.setVisibleColumns();
  }

}
