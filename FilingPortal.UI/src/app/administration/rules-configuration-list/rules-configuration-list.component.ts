import { Component, OnInit, ViewChild, TemplateRef, HostListener, NgZone, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import * as R from 'ramda';

import { ModalService, EventsService, ConfigurationService } from '@common/services';
import { GridPageApiService, GridPageColumnsService, GridService, GridStorageService } from '@common/grid/services';
import { AddFieldOptionsToModelHandler, GridDataHandler, AddViewModeToModelHandler } from '@common/grid/handlers';
import { RulesConfigurationService } from '../services';

import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { AvailableActions } from '@common/models';
import { ToastrService } from 'ngx-toastr';
import { LayoutService } from '@common/services/layout.service';
import { FieldsService } from '@common/fields/services/fields.service';


@Component({
  selector: 'lxft-rules-configuration-list',
  templateUrl: './rules-configuration-list.component.html',
})
export class RulesConfigurationListComponent extends GridPageCtrl implements OnInit {
  private editRow: any;
  private addFieldOptionsToModelHandler = new AddFieldOptionsToModelHandler();

  @ViewChild('actionsTmpl')
  actionsTmpl: TemplateRef<any>;
  @ViewChild('editableColumnTmpl')
  editableColumnTmpl: TemplateRef<any>;

  public filterConfigName: string;
  public isWorking: boolean;
  public availableActions: AvailableActions;

  public get isEdit(): boolean {
    return !!this.editRow;
  }

  @HostListener('window:beforeunload')
  hasNotChanged(): Observable<boolean> | boolean {
    return !this.isEdit;
  }

  constructor(
    private route: ActivatedRoute,
    protected api: GridPageApiService,
    protected columnsService: GridPageColumnsService,
    protected gridService: GridService,
    protected gridStorageService: GridStorageService,
    private modalService: ModalService,
    private toastr: ToastrService,
    private rulesConfigurationService: RulesConfigurationService,
    private configurationService: ConfigurationService,
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    private fieldsService: FieldsService
  ) {
    super(api, columnsService, gridService, gridStorageService, eventsService, cdr, layoutService);

    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addViewModeToModelHandler',
        AddViewModeToModelHandler.handler
      )
    );
    // todo: In case of a performance drop, set only for the line being edited
    this.dataHandlers.add(
      new GridDataHandler<any>(
        'addFieldOptionsToModelHandler',
        this.addFieldOptionsToModelHandler.handler
      )
    );
  }

  ngOnInit() {
    super.ngOnInit();
    this.autoHeight = true;
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
      this.configurationService
        .getPageActions(pageName)
        .subscribe(result => (this.availableActions = result));
    }
  }

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.rulesConfigurationService.getGridPageSettings(name);
    return !!this.paginationOptions;
  }

  async setSort(pageInfo) {
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
      const result = await this.modalService
        .confirm({
          text: 'Are you sure that you want to discard changes for the row?'
        });
      return (result ? true : false);
    } else {
      return Promise.resolve(true);
    }
  }

  updateGridColumns(): void {
    this.addFieldOptionsToModelHandler.Columns = [...this.columns];
    this.addFieldOptionsToModelHandler.GridName = this.paginationOptions.gridName;
    this.setColumnTmplt();
    this.setActionsGridColumn();
  }

  setColumnTmplt(): void {
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
    this.registerMissingDependancy(row);

    row.ViewMode = false;
  }

  private registerMissingDependancy(row: any) {
    this.columns.forEach(c => {
      if (!c.type) {
        this.fieldsService.register(c.name, () => row[c.name], x => row[c.name] = x);
      }
    });
  }

  delete(rowId: number): void {
    if (!this.isEdit) {
      this.modalService
        .confirm({ text: 'Are you sure that you want to delete the rule?' })
        .then(confirmed => {
          if (confirmed) {
            this.isWorking = true;
            this.rulesConfigurationService
              .delete(this.paginationOptions.pathForApi, rowId)
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
    this.rulesConfigurationService
      .update(this.paginationOptions.pathForApi, row)
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
        this.isWorking = true;
        this.rulesConfigurationService
          .getNew(this.paginationOptions.pathForApi)
          .finally(() => {
            this.isWorking = false;
          })
          .subscribe(result => {
            this.addEditRow(result);
          });
      } else {
        this.addEditRow(this.rulesConfigurationService.depersonalize(templateRow));
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

  create(row) {
    this.isWorking = true;
    this.rulesConfigurationService
      .add(this.paginationOptions.pathForApi, row)
      .finally(() => {
        this.isWorking = false;
      })
      .subscribe(result => {
        this.handleValidationResult(result, row);
      });
  }

  private handleValidationResult(result, row: any) {
    if (result.IsValid) {
      this.getPage();
      this.emptyEditRow();
    } else {
      result.FieldsErrors.forEach(error => {
        if (row.options && row.options[error.FieldName]) {
          row.options[error.FieldName].validationOn = true;
          row.options[error.FieldName].errors = [error.Message];
        }
        if (error.Message) {
          this.toastr.error(
            `${error.FieldName}: ${error.Message}`
          );
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
      }
    });
  }

  restoreRow(): void {
    this.unregisterMissingDependancy();
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
      this.emptyEditRow();
    }
  }

  private unregisterMissingDependancy() {
    this.columns.forEach(c => {
      if (!c.type) {
        this.fieldsService.unregister(c.name);
      }
    });
  }

  emptyEditRow() {
    this.editRow = undefined;
  }

}
