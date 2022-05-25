import { Component, OnInit, ViewChild, TemplateRef, ChangeDetectorRef, Inject } from '@angular/core';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { IGridsConfigurationService } from '@common/interfaces';
import { ActivatedRoute, Router } from '@angular/router';
import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '@common/grid/services';
import { ModalService, EventsService, LayoutService, ConfigurationService } from '@common/services';
import { ToastrService } from 'ngx-toastr';
import { AuditApiService } from '@app/audit/services/audit-api.service';
import { GridDataHandler } from '@common/grid/handlers';
import { AddUserHighlightingTypePropertyToModelHandler } from '@inbound/handlers';
import { styleMappHighlightingType } from '@app/utils';
import { IconType } from '@common/icon-tooltip';
import { FieldsValidationResult, HighlightingType, Severity } from '@common/models';
import { Filter } from '@common/filters/models';

@Component({
  selector: 'lxft-daily-audit',
  templateUrl: './daily-audit.component.html'
})
export class DailyAuditComponent extends GridPageCtrl implements OnInit {

  @ViewChild('notificationIconTmpl')
  notificationIconTmpl: TemplateRef<any>;
  @ViewChild('actionsTmpl')
  public actionsTmpl: TemplateRef<any>;
  @ViewChild('editableColumnTmpl')
  editableColumnTmpl: TemplateRef<any>;

  protected get actionsTemplate(): TemplateRef<any> {
    return this.actionsTmpl;
  }

  public isWorking: boolean;
  IconType = IconType;
  Severity = Severity;

  constructor(protected route: ActivatedRoute,
    protected router: Router,
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
    private apiService: AuditApiService
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

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.configurationService.getGridConfig(name);
    return !!this.paginationOptions;
  }

  setPageAvailableActions(name: string): void {
    const pageName = this.configurationService.getPageActionsConfig(name);
    if (pageName) {
      this.pageActionsService.getPageActions(pageName).subscribe(result => this.availableActions = result);
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
    this.setColumnTmplts();
    this.setActionsGridColumn();
    this.setNotificationColumn();
  }

  private setFirstColumnBorder() {
    if (this.columns && this.columns.length) {
      this.columns[0] = this.hideColumnBorder(this.columns[0]);
    }
  }

  setColumnTmplts(): void {
    this.columns.forEach(column => {
      column.cellTemplate = this.editableColumnTmpl;
    });
  }

  protected setActionsGridColumn() {
    const actionCol = this.columnsService.getActionColumn(this.actionsTemplate);
    this.columns.push(actionCol);
  }

  private setNotificationColumn() {
    const field = this.columnsService.getNotificationColumn(this.notificationIconTmpl);
    field.maxWidth = 60;
    field.minWidth = 60;
    field.width = 60;
    this.columns.push(this.hideColumnBorder(field));
  }

  getIconTypeFor(row: any, severity: Severity): IconType {
    if (severity === Severity.Error) {
      if (this.getErrors(row).length > 0) { return IconType.Error; }
    }
    if (severity === Severity.Warning) {
      if (this.getWarnings(row).length > 0) { return IconType.Warning; }
    }
    return null;
  }

  public editRule(row: { ImporterCode: string, Tariff: string }): void {
    this.router.navigate(
      ['daily-audit-rules',
        {
          'filter-ImporterCode': row.ImporterCode,
          'filter-Tariff': row.Tariff
        }],
      { relativeTo: this.route.parent });
  }

  getErrors(row: { Errors: FieldsValidationResult[] }): string[] {
    return row.Errors.filter(x => x.Severity === Severity.Error).map(x => x.Message);
  }

  getWarnings(row: { Errors: FieldsValidationResult[] }): string[] {
    return row.Errors.filter(x => x.Severity === Severity.Warning).map(x => x.Message);
  }

  report(filters?: Filter[]) {
    this.setFilters(filters);
    this.apiService.report(this.getPageParams()).subscribe(() => {
      this.getTotalMatches();
      this.getPage();
    });
  }
}
