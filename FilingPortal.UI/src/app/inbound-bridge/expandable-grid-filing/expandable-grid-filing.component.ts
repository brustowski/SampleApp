import { Component, OnInit, ViewChild, TemplateRef, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { GridService, GridStorageService, GridPageColumnsService, GridPageApiService } from '@common/grid/services';
import { EventsService, ModalService } from '@common/services';
import { InboundConfigurationService, FilingParametersService } from '@inbound/services';
import { LayoutService } from '@common/services/layout.service';
import { Filter } from '@common/filters/models';
import { TypedErrorsList, FieldErrors } from '@inbound/models';
import { styleMappHighlightingType } from '@app/utils';
import { IconType } from '@common/icon-tooltip';
import * as R from 'ramda';
import { DatatableComponent } from '@custom/ngx-datatable';
import { FilingHeaderConfirmation, HighlightingType } from '@common/models';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lxft-expandable-grid-filing',
  templateUrl: './expandable-grid-filing.component.html'
})
export class ExpandableGridFilingComponent extends GridPageCtrl implements OnInit {
  private filingHeaderIds: number[];

  @ViewChild('docsTmpl')
  documentsTemplate: TemplateRef<any>;

  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;

  @ViewChild('radioTmpl')
  radioTemplate: TemplateRef<any>;

  @ViewChild('confirmationTmpl')
  confirmationTmpl: TemplateRef<any>;

  @ViewChild('confirmationHeaderTmpl')
  confirmationHeaderTmpl: TemplateRef<any>;

  @ViewChild('expandTmpl')
  expandTemplate: TemplateRef<any>;

  @ViewChild('excludeTmpl')
  excludeTmpl: TemplateRef<any>;

  @Output() onSelected: EventEmitter<number> = new EventEmitter();
  @Output() onExclude: EventEmitter<number> = new EventEmitter();

  public IconType = IconType;

  private lastExpandedRowId: string | number = null;
  public viewMode: boolean;
  public get confirmed(): boolean {
    return R.all<any>(x => x.Confirmed, this.rows);
  }

  constructor(
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected events: EventsService,
    protected columnsService: GridPageColumnsService,
    protected api: GridPageApiService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private configuration: InboundConfigurationService,
    private filingService: FilingParametersService,
    private route: ActivatedRoute,
    private modal: ModalService,
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.viewMode = this.route.snapshot.data['viewMode'];
    this.filingHeaderIds = this.filingService.filingHeaderIds;

    this.paginationOptions = this.configuration.getSingleFilingPageConfiguration();

    const filter = new Filter();
    filter.fieldName = 'FilingHeaderId';
    filter.value = this.filingHeaderIds;
    filter.operand = 'equals';

    this.filtersOptions = [filter];
    this.initGridConfigAndData();
  }

  updateGridColumns() {
    this.setCheckboxColumns();
    this.setNotificationColumn();
    this.setDocumentsColumn();
    if (this.filingService.ConfirmationAvailable && !this.viewMode) {
      this.setConfirmationColumn();
    }
    if (this.filingService.CanExclude && !this.viewMode) {
      this.setExcludeColumn();
    }
  }

  setCheckboxColumns() {
    const checkboxField = this.columnsService
      .createColumn()
      .fieldName('checkbox')
      .sortable(false)
      .align(2)
      .cellTemplate(this.radioTemplate)
      .minWidth(46)
      .maxWidth(46)
      .build();
    checkboxField.headerClass = 'no-border';
    checkboxField.cellClass = 'no-border';
    this.columns.unshift(checkboxField);
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    this.columns.unshift(field);
  }

  private setExcludeColumn() {
    const excludeColumn = this.columnsService
      .createColumn()
      .displayName('')
      .fieldName('exclude')
      .sortable(false)
      .cellTemplate(this.excludeTmpl)
      .maxWidth(100)
      .build();

    this.columns.push(excludeColumn);
  }

  private setConfirmationColumn() {
    const actionsField = this.columnsService
      .createColumn()
      .fieldName('confirmation')
      .sortable(false)
      .align(2)
      .cellTemplate(this.confirmationTmpl)
      .headerTemplate(this.confirmationHeaderTmpl)
      .minWidth(46)
      .maxWidth(46)
      .build();

    this.columns.push(actionsField);
  }

  setDocumentsColumn(): void {
    const docsColumn = this.columnsService
      .createColumn()
      .displayName('')
      .fieldName('DocsAmount')
      .sortable(false)
      .cellTemplate(this.documentsTemplate)
      .maxWidth(100)
      .build();

    this.columns.push(docsColumn);
  }

  getRowId(row: { FilingHeaderId: number }): number {
    return row.FilingHeaderId;
  }

  getRowClass = (row: TypedErrorsList<FieldErrors>): string =>
    row.Errors && row.Errors.length
      ? styleMappHighlightingType[HighlightingType.Warning]
      : styleMappHighlightingType[HighlightingType.NoHighlighting];

  getIconTypeFor = (row: TypedErrorsList<FieldErrors>): IconType =>
    row.Errors && row.Errors.length ? IconType.Warning : IconType.None;

  getErrorsFor = (row: TypedErrorsList<FieldErrors>): string[] =>
    row.Errors && row.Errors.length ? row.Errors.reduce((r: string[], e: FieldErrors) => r.concat(e.Errors), []) : [];

  getValidationStatus(): { FilingHeaderId: number; IsValid: boolean }[] {
    return this.rows.map(row => ({
      FilingHeaderId: row.FilingHeaderId,
      IsValid: row.Errors.length === 0
    }));
  }

  updateRowStatus(fieldErrors: FieldErrors[]): void {
    const selectedRow: TypedErrorsList<FieldErrors> = this.selectedRows[0];
    if (selectedRow) {
      const defaultToArr = R.defaultTo([]);
      const cmp = (a: FieldErrors, b: FieldErrors) => a.FieldId === b.FieldId;
      const fltrFn = (a: FieldErrors): boolean => !R.isEmpty(a.Errors);
      const diffErrors: FieldErrors[] = R.differenceWith(cmp, defaultToArr(selectedRow.Errors), fieldErrors);
      const newErrors: FieldErrors[] = R.filter(fltrFn, fieldErrors);
      selectedRow.Errors = R.concat(diffErrors, newErrors);
    }
  }

  onSelect(row: { selected: { FilingHeaderId: number }[] }): void {
    if (row && !(row instanceof Event)) {
      this.toggleExpandRow(row.selected[0]);
    }
  }

  toggleExpandRow(row) {
    this.gridTable.rowDetail.collapseAllRows();
    this.selectedRows.splice(0, this.selectedRows.length); // Clear selected rows
    if (this.getRowId(row) !== this.lastExpandedRowId) {
      this.gridTable.rowDetail.toggleExpandRow(row);
      this.lastExpandedRowId = this.getRowId(row);
      this.selectedRows.push(row);
    } else {
      this.lastExpandedRowId = null;
    }
  }

  protected onGridRendered(grid: DatatableComponent) {
    super.onGridRendered(grid);
    if (this.rows.length === 1) {
      this.toggleExpandRow(this.rows[0]);
      grid.rowDetail.expandAllRows();
    }
  }

  onConfirm(event: Event, row: { FilingHeaderId: number, Confirmed: boolean }) {
    if (event) {
      event.stopPropagation();
    }
    row.Confirmed = !row.Confirmed;
    this.confirm([row]);
  }

  onConfirmAll(value: boolean) {
    const rows = this.rows.filter(x => x.Confirmed !== value);

    rows.forEach(x => x.Confirmed = value);
    this.confirm(rows);
  }

  private confirm(rows: { FilingHeaderId: number, Confirmed: boolean }[]) {
    if (!this.viewMode) {
      const confirmationRequest: FilingHeaderConfirmation[]
        = rows.map(x => <FilingHeaderConfirmation>{ FilingHeaderId: this.getRowId(x), Confirmed: x.Confirmed });
      this.filingService.updateConfirmationStatus(confirmationRequest).subscribe(serverValue => {
        serverValue.forEach(c => {
          const row = this.rows.find(r => this.getRowId(r) === c.FilingHeaderId);
          row.Confirmed = c.Confirmed;
        });
      });
    }
  }

  exclude(event: Event, row: { FilingHeaderId: number }) {
    if (event) {
      event.stopPropagation();
    }
    const message = 'Are you sure to exclude this record from creation process?';
    this.modal.confirm({ text: message }).then(confirmed => {
      if (confirmed) {
        const rowIndex = this.rows.indexOf(row);
        this.rows.splice(rowIndex, 1);
        const filingHeaderIndex = this.filingService.filingHeaderIds.indexOf(row.FilingHeaderId);
        this.filingService.filingHeaderIds.splice(filingHeaderIndex, 1);
        this.onExclude.emit();
      }
    });
  }
}
