import { Component, OnInit, ChangeDetectorRef, Inject, ViewChild, TemplateRef, OnDestroy } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { ActivatedRoute } from '@angular/router';
import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '@common/grid/services';
import { ModalService, EventsService, LayoutService, ConfigurationService, FileUploadService } from '@common/services';
import { ToastrService } from 'ngx-toastr';
import { IGridsConfigurationService } from '@common/interfaces';
import { GridDataHandler } from '@common/grid/handlers';
import { AddUserHighlightingTypePropertyToModelHandler } from '@inbound/handlers';
import { ReconApiService } from '../services/recon-api.service';
import { Filter } from '@common/filters/models';
import { Column, ColumnConfiguration } from '@common/grid/models';
import { styleMappHighlightingType, typeMappHighlightingIcon } from '@app/utils';
import { IconType } from '@common/icon-tooltip';
import { FileUploadResultComponent } from '@common/file-uploader';
import { AvailableActions, HighlightingType, ReconGridNames } from '@common/models';
import * as R from 'ramda';
import { ReconService } from '../services/recon.service';

@Component({
  selector: 'lxft-recon-cargowise-list',
  templateUrl: './cargowise-list.component.html'
})
export class CargoWiseListComponent extends GridPageCtrl implements OnInit, OnDestroy {

  @ViewChild('columnTemplate')
  columnTemplate: TemplateRef<any>;
  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;

  public isWorking: boolean;
  public uploadUrl: string;
  public pageAvailableActions: AvailableActions = {};

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
    @Inject('ReconGridsConfigurationService') private configurationService: IGridsConfigurationService,
    private pageActionsService: ConfigurationService,
    private reconApiService: ReconApiService,
    protected fileService: FileUploadService,
    private reconService: ReconService

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
  }

  ngOnDestroy() {
    this.reconService.setActions({});
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
        this.uploadUrl = `imports/upload/grids/${ReconGridNames.AceReport}`;
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

  protected setActionsGridColumn() {
    const actionCol = this.columnsService.getActionColumn(this.actionsTmpl);
    this.columns.push(actionCol);
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    this.columns.push(this.hideColumnBorder(field));
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

  report(filters?: Filter[]) {
    this.setFilters(filters);
    this.reconApiService.report(this.getPageParams()).subscribe(() => {
      this.getTotalMatches();
      this.getPage();
    });
  }

  getRowId(row: any): number | string {
    return row.Id;
  }

  canBeChecked(row: any): boolean { return true; }

  onSelect(rows: { selected: any[] }) {
    this.selectedRows = rows.selected;
    this.refreshGrid();
  }

  highlightCell(row: any, column: Column): boolean {
    return row[`Mismatch${column.prop}`];
  }

  getRequiredValue(row: any, column: Column): any {
    const value = row[`Ace${[column.prop]}`];
    return R.isNil(value) ? '' : `(${value})`;
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

  getIconTypeFor(row: any): IconType {
    const highlightingType = this.getRowHighlighting(row);
    const iconType = typeMappHighlightingIcon.find(m => m.highlightingType === highlightingType);
    return iconType ? iconType.iconType : IconType.None;
  }

  importAceReport($event: any): void {
    this.getPage();
    const result = this.fileService.parseFileUploadResultResponse($event);
    this.modalService.open(FileUploadResultComponent, result);
  }

  onUploadError($event: any): void {
    this.toastr.error($event);
  }

  exportRecon(): void {
    this.reconApiService.exportAceReport(this.getPageParams());
  }

  resetReport(): void {
    this.reconApiService.resetReport().subscribe(() => this.getPage());
  }
}
