import { ViewChild, TemplateRef, ChangeDetectorRef, OnInit, Input } from '@angular/core';

import * as R from 'ramda';
import 'rxjs/add/operator/map';

import { DatatableComponent } from '@custom/ngx-datatable';

import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '../services';

import { Page, Sort, Column, ColumnConfiguration } from '../models';
import { Filter } from '@common/filters/models';

import { GridDataHandlerCollection } from '../handlers';
import { EventsService, LayoutService } from '@common/services';
import { isUndefined } from 'util';
import { AvailableActions } from '@common/models';

export class GridPageCtrl implements OnInit {
  private _grid: DatatableComponent = null;

  public columns: Array<Column>;
  public visibleColumns: Array<Column> = [];
  public rows = [];
  public summaryRow: any;
  public selectedRows = [];
  public paginationOptions: Page = new Page();
  public sortOptions: Sort = new Sort();
  public filtersOptions: Filter[];
  public isLoadingFirst: boolean = false;
  public defSortGrid: Array<{ prop: string; dir: string }> = [];
  public selectAllEnabled: boolean = true;
  protected dataHandlers: GridDataHandlerCollection<any>;
  public availableActions: AvailableActions = {};

  @Input()
  public autoHeight: boolean = false;

  @ViewChild('clickableColumnTmpl') clickableColumnTmpl: TemplateRef<any>;
  @ViewChild('gridTable')
  set gridTable(grid: DatatableComponent) {
    if (grid !== this._grid) {
      this._grid = grid;
      if (grid) {
        this.onGridRendered(grid);
      }
    }
  }
  get gridTable(): DatatableComponent {
    return this._grid;
  }

  constructor(
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    private layoutsService: LayoutService
  ) {
    this.isLoadingFirst = true;
    this.dataHandlers = new GridDataHandlerCollection();
  }

  ngOnInit(): void {
    addEventListener('resize', () => {
      this.recalculateGridSize();
    });

    this.eventsService.updateGridSize$.subscribe(() => {
      this.recalculateGridSize();
    });
  }

  setPageNumber(newPageNumber) {
    this.paginationOptions.pageNumber = newPageNumber;
    this.getPage();
  }

  setSort(pageInfo) {
    this.sortOptions.column = pageInfo.column.prop;
    this.sortOptions.order = pageInfo.newValue;
    this.gridStorageService.setSorting(this.sortOptions, this.paginationOptions.gridName);
    this.getPage();
  }

  setFilters(filters?: Filter[]) {
    this.filtersOptions = R.isEmpty(filters) || R.isNil(filters) ? [] : filters;
    this.paginationOptions.pageNumber = 1;

    this.getTotalMatches();
    this.getPage();
  }

  setPageSize(pageSize) {
    this.paginationOptions.size = pageSize;
    this.paginationOptions.pageNumber = 1;
    this.gridStorageService.setPageSize(pageSize, this.paginationOptions.gridName);
    this.getPage();
  }

  getPage() {
    this.api
      .listSearch(this.paginationOptions.pathForApi, this.getPageParams())
      .then(data => {
        this.setList(data);
        this.recalculateGridSize();
        this.setSelectAllStatus();
        this.getPageExtended();
        this.isLoadingFirst = false;
      })
      .catch(() => {
        this.isLoadingFirst = false;
      });
  }

  getPageParams() {
    return this.gridService.getPageParams(this.paginationOptions, this.sortOptions, this.filtersOptions);
  }

  setList(data) {
    const rows = this.parseList(data);
    this.summaryRow = this.parseSummary(data);
    this.dataHandlers.handleMultiple(rows);
    this.rows = rows;
    this.paginationOptions.pageNumber = data.CurrentPage;
    this.paginationOptions.rowsOnPage = this.rows.length;
  }

  parseList(data) {
    return data.Results;
  }

  parseSummary(data) {
    return data.Summary;
  }

  getPageExtended(): void { }

  updateGridColumns() { }

  initGridConfigAndData() {
    if (!this.paginationOptions) {
      throw new Error('Grid configuration not found');
    }
    this.getGridColumnsConfig().subscribe(() => {
      this.getTotalMatches();
      this.getPage();
    });
  }

  getGridColumnsConfig() {
    return this.api.getGridColumnsConfig(this.paginationOptions.gridName).map(data => {
      this.columns = data.map(item => this.columnsService.parseGridColumn(item));
      this.updateGridColumns();
      this.restoreSettings();
      this.setVisibleColumns();
    });
  }

  restoreSettings() {
    const config = this.gridStorageService.getConfiguration(this.paginationOptions.gridName);
    if (config) {
      this.setDefaultSorting(config.sorting);
      this.setDefaultPageSize(config.pageSize);
      this.setDefaultColumns(config.columns);
    }
  }

  setDefaultSorting(sortSettings: Sort) {
    if (!sortSettings || this.columns.filter(x => !x.isSystem).findIndex(x => x.prop === sortSettings.column) === -1) {
      const column = this.columns.find(item => item.isDefaultSort);
      if (!column) {
        return;
      }
      sortSettings = { column: column.prop, order: 'asc' };
    }
    const newSort = {
      prop: sortSettings.column,
      dir: <string>sortSettings.order
    };
    this.defSortGrid = [newSort];
    this.sortOptions = sortSettings;
  }

  setDefaultPageSize(size: number): void {
    if (size) {
      this.paginationOptions.size = size;
    }
  }

  setDefaultColumns(configs: ColumnConfiguration[]) {
    if (configs) {
      this.columns.forEach(column => {
        const config = configs.find(c => c.prop === column.prop);
        if (config) {
          if ((isUndefined(column.minWidth) || config.width >= column.minWidth) &&
            (isUndefined(column.maxWidth) || config.width <= column.maxWidth)
          ) {
            column.width = config.width;
            column.canAutoResize = false;
          }
          column.isVisible = config.isVisible;
        }
      });
    }
  }

  setVisibleColumns(): void {
    this.visibleColumns = this.columns.filter(x => x.isVisible);
  }

  getTotalMatches() {
    this.paginationOptions.isMatchLoading = true;
    this.api.getTotalMatches(this.paginationOptions.pathForApi, this.getPageParams()).then(data => {
      this.paginationOptions.filteredRows = data;
      this.paginationOptions.isMatchLoading = false;
    });
  }

  setClickableColumns() {
    this.columns = this.columns.map(item => {
      if (item.isViewOpen) {
        item.cellTemplate = this.clickableColumnTmpl;
      }
      return item;
    });
  }

  setCheckboxColumns() {
    const checkboxField = this.columnsService.setCheckboxColumn();
    this.columns.push(checkboxField);
  }

  clearSelected() {
    this.selectedRows = [];
  }

  protected recalculateGridSize() {
    setTimeout(() => {
      if (this.gridTable) {
        const currentHeight = this.gridTable.fullHeight;
        const availableHeight = this.layoutsService.getGridAvailableHeight();
        const useCurrentHeight = currentHeight <= availableHeight;

        if (this.autoHeight) {
          this.gridTable.element.style.height = (useCurrentHeight ? currentHeight : availableHeight) + 'px';
        }
        this.gridTable.recalculateAndUpdateView();

        // grid full height may change through this process because horizontal scroll may appear or disappear
        // In this case we need to recheck control height
        setTimeout(() => {
          if (this.autoHeight && useCurrentHeight && currentHeight !== this.gridTable.fullHeight) {
            this.recalculateGridSize();
          }
        }, 100);
      }
    });
  }

  protected onGridRendered(grid: DatatableComponent) {
    this.recalculateGridSize();
  }

  /**
   * Column resize event handler
   * @param event Column resize event { column: Column; newValue: number }
   */
  onColumnResize(event: { column: Column; newValue: number }): void {
    this.gridStorageService.setColumnSize(event.column.prop, event.newValue, this.paginationOptions.gridName);
    this.columns
      .filter(column => column.prop === event.column.prop)
      .forEach(column => {
        column.canAutoResize = false;
        column.width = event.newValue;
      });
    this.columns = [... this.columns];
  }

  /**
   * Manual grid refresh
   */
  refreshGrid(): void {
    this.dataHandlers.handleMultiple(this.rows);
    this.rows = R.clone(this.rows);
  }

  canBeChecked(row: any): boolean {
    return row.Actions.Select && row.UserActions.Select;
  }

  setSelectAllStatus(): void {
    this.selectAllEnabled = this.rows.every(x => this.canBeChecked(x)) && this.dataHandlers.checkMultiple(this.rows);
  }

  protected hideColumnBorder(column: Column): Column {
    const newColumn = column;
    newColumn.headerClass = 'no-border';
    newColumn.cellClass = 'no-border';
    return newColumn;
  }

  protected setHeaderTemplate(column: Column, tmplt: TemplateRef<any>): void {
    column.headerTemplate = tmplt;
  }

  stringifyValue(value: any, column: { type: string; columns: Column[]; }): string {
    if (column.type === 'composite') {
      return column.columns.map((x: Column) => value[x.prop]).join(' ');
    }
    return value;
  }
}
