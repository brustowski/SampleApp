import { Component, OnInit, ViewChild, TemplateRef, HostListener, ChangeDetectorRef, ElementRef } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/finally';

import * as R from 'ramda';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';

import { GridPageApiService, GridPageColumnsService, GridStorageService, GridService } from '@common/grid/services';
import { RulesApiService } from '../services/rules-api.service';
import { ModalService } from '@common/services/modal.service';

import { AvailableActions, ValidationResultWithFieldsErrorsViewModel } from '@common/models';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { AddFieldOptionsToModelHandler, GridDataHandler, AddViewModeToModelHandler } from '@common/grid/handlers';

import { RulesService } from '../services/rules.service';
import { EventsService } from '@common/services/events.service';
import { RulesConfigurationService } from '../services';
import { ConfigurationService } from '@common/services';
import { LayoutService } from '@common/services/layout.service';
import { Column } from '@common/grid/models';

@Component({
  selector: 'lxft-rule-list',
  templateUrl: './rule-list.component.html'
})
export class RuleListComponent extends GridPageCtrl implements OnInit {
  private editRow: any;
  private addFieldOptionsToModelHandler = new AddFieldOptionsToModelHandler();

  @ViewChild('actionsTmpl')
  actionsTmpl: TemplateRef<any>;
  @ViewChild('editableColumnTmpl')
  editableColumnTmpl: TemplateRef<any>;
  @ViewChild('hdrTpl')
  headerTemplate: TemplateRef<any>;
  @ViewChild('fileInput')
  public fileInput: ElementRef;

  public uploader: FileUploader;

  public filterConfigName: string;
  public isWorking: boolean;
  public availableActions: AvailableActions;

  public get isEdit(): boolean {
    return !!this.editRow;
  }

  public uploadUrl = `/imports/uploads`;

  @HostListener('window:beforeunload')
  hasNotChanged(): Observable<boolean> | boolean {
    return !this.isEdit;
  }

  constructor(
    protected route: ActivatedRoute,
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    protected modalService: ModalService,
    protected toastr: ToastrService,
    protected rulesApiService: RulesApiService,
    protected rulesService: RulesService,
    protected rulesConfigurationService: RulesConfigurationService,
    protected configurationService: ConfigurationService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);

    this.dataHandlers.add(new GridDataHandler<any>('addViewModeToModelHandler', AddViewModeToModelHandler.handler));
    // todo: In case of a performance drop, set only for the line being edited
    this.dataHandlers.add(
      new GridDataHandler<any>('addFieldOptionsToModelHandler', this.addFieldOptionsToModelHandler.handler)
    );
  }

  ngOnInit() {
    super.ngOnInit();
    this.autoHeight = true;
    this.loadInitialParameters();
  }

  protected loadInitialParameters(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      const pageName = params.get('name');
      this.isLoadingFirst = true;
      if (this.setPaginationOptions(pageName)) {
        if (this.isEdit) {
          this.restoreRow();
        } else {
          this.emptyEditRow();
        }
        this.initGridConfigAndData();
      }
      this.setPageAvailableActions(pageName);
    });
  }

  setPageAvailableActions(name: string): void {
    const pageName = this.rulesConfigurationService.getPageActionsConfig(name);
    if (pageName) {
      this.configurationService.getPageActions(pageName).subscribe(result => (this.availableActions = result));
    }
  }

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.rulesConfigurationService.getGridConfig(name);
    return !!this.paginationOptions;
  }

  async setSort(pageInfo: { column: { prop: any; }; newValue: any; }) {
    this.defSortGrid = [{ prop: this.sortOptions.column, dir: this.sortOptions.order.toString() }];
    const result = await this.canDeactivate();
    if (result) {
      if (this.isEdit) {
        this.emptyEditRow();
      }
      this.defSortGrid = [{ prop: pageInfo.column.prop, dir: pageInfo.newValue }];
      super.setSort(pageInfo);
    } else {
    }
  }

  async canDeactivate(): Promise<boolean> {
    if (this.isEdit) {
      const result = await this.modalService.confirm({
        text: 'Are you sure that you want to discard changes for the row?'
      });
      return result ? true : false;
    } else {
      return Promise.resolve(true);
    }
  }

  updateGridColumns(): void {
    this.addFieldOptionsToModelHandler.Columns = [...this.columns];
    this.addFieldOptionsToModelHandler.GridName = this.paginationOptions.gridName;
    this.setColumnTmplts();
    this.setActionsGridColumn();
    this.setHeaderTemplates();
  }

  setHeaderTemplates(): void {
    this.columns.forEach(x => {
      if (x.isKeyField) {
        this.setHeaderTemplate(x, this.headerTemplate);
        x.headerClass = 'key-field';
      }
    });
  }

  setColumnTmplts(): void {
    this.columns.forEach(column => {
      column.cellTemplate = this.editableColumnTmpl;
    });
  }

  setActionsGridColumn(): void {
    const actionCol = this.columnsService.getActionColumn(this.actionsTmpl);
    actionCol.width = 135;
    actionCol.maxWidth = 135;
    actionCol.frozenRight = true;
    this.columns.push(actionCol);
  }

  edit(row: any): void {
    this.editRow = R.clone(row);

    row.ViewMode = false;
  }

  delete(rowId: number): void {
    if (!this.isEdit) {
      this.modalService.confirm({ text: 'Are you sure that you want to delete the rule?' }).then(confirmed => {
        if (confirmed) {
          this.isWorking = true;
          this.rulesApiService
            .deleteRule(this.paginationOptions.pathForApi, rowId)
            .finally(() => {
              this.isWorking = false;
            })
            .subscribe(result => {
              if (!result.IsValid) {
                this.toastr.error(result.CommonError);
              }
              this.getPage();
            });
        }
      });
    }
  }

  update(row: any): void {
    this.isWorking = true;
    this.rulesApiService
      .updateRule(this.paginationOptions.pathForApi, row)
      .finally(() => {
        this.isWorking = false;
      })
      .subscribe(result => {
        this.handleValidationResult(result, row);
        if (result.CommonError && result.FieldsErrors.length === 0) {
          this.getPage();
          this.emptyEditRow();
        }
      });
  }

  add(templateRow?: any) {
    if (!this.isEdit) {
      if (!templateRow) {
        // We create row without template
        this.rulesApiService
          .getNewRule(this.paginationOptions.pathForApi)
          .finally(() => {
            this.recalculateGridSize();
          })
          .subscribe(result => {
            this.addEditRow(result);
          });
      } else {
        this.addEditRow(this.rulesService.depersonalizeRule(templateRow));
        this.recalculateGridSize();
      }
    }
  }

  private addEditRow(rowData: any) {
    this.eventsService.scrollTo(0);
    this.paginationOptions.rowsOnPage++;
    this.dataHandlers.handleMultiple([rowData]);
    this.rows = [rowData, ...this.rows];
    this.edit(this.rows[0]);
  }

  create(row: any) {
    this.isWorking = true;
    this.rulesApiService
      .createRule(this.paginationOptions.pathForApi, row)
      .finally(() => {
        this.isWorking = false;
      })
      .subscribe(result => {
        this.handleValidationResult(result, row);
      });
  }

  private handleValidationResult(result: ValidationResultWithFieldsErrorsViewModel, row: any) {
    if (result.IsValid) {
      this.getPage();
      this.emptyEditRow();
    } else {
      result.FieldsErrors.forEach((error: { FieldName: string; Message: any; }) => {
        const column = this.columns.find(x => x.updatedProperty === error.FieldName);
        if (column && row.options && row.options[column.prop]) {
          row.options[column.prop].validationOn = true;
          row.options[column.prop].errors = [error.Message];
        }
        if (error.Message) {
          this.toastr.error(`${error.Message}`);
        }
      });
      if (result.CommonError) {
        this.toastr.error(result.CommonError);
      }
    }
  }

  cancel(): void {
    this.canDeactivate().then(result => {
      if (result) {
        this.restoreRow();
        this.recalculateGridSize();
      }
    });
  }

  restoreRow(): void {
    if (this.editRow.Id === 0) {
      this.paginationOptions.rowsOnPage--;
      const rows = this.rows.slice(1);
      this.rows = [...rows];
      this.emptyEditRow();
      return;
    }
    const indx = this.rows.findIndex(r => r.Id === this.editRow.Id);
    if (indx > -1) {
      this.rows[indx] = R.clone(this.editRow);
      this.rows = [...this.rows];
      this.emptyEditRow();
    }
  }

  emptyEditRow() {
    this.editRow = undefined;
  }

  public getLookupValue(row: any, propertyName: string) {
    return row[`${propertyName}Id`] || row[propertyName];
  }

  public setLookupValue(row: any, propertyName: string, value: string) {
    if (`${propertyName}Id` in row) {
      row[`${propertyName}Id`] = value;
    }
    row[propertyName] = value;
  }

  onUploadSuccess(): void {
    this.getPage();
  }

  onUploadError($event: any): void {
    this.toastr.error($event);
  }

  downloadTemplate(): void {
    this.api.downloadRuleTemplate(this.paginationOptions.gridName);
  }
}
