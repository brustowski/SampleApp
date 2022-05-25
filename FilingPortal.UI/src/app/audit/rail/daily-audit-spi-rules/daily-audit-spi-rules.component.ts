import { Component, OnInit, ViewChild, TemplateRef, HostListener, NgZone, ChangeDetectorRef, Inject, ElementRef } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/finally';

import * as R from 'ramda';

import { GridPageApiService, GridPageColumnsService, GridStorageService, GridService } from '@common/grid/services';
import { ModalService } from '@common/services/modal.service';

import { AvailableActions } from '@common/models';
import { GridPageCtrl } from '@common/grid/grid-page/grid-page-ctrl';
import { AddFieldOptionsToModelHandler, GridDataHandler, AddViewModeToModelHandler } from '@common/grid/handlers';

import { EventsService } from '@common/services/events.service';
import { ConfigurationService, FileUploadService } from '@common/services';
import { ToastrService } from 'ngx-toastr';
import { LayoutService } from '@common/services/layout.service';
import { RulesApiService, RulesService } from '@app/rules/services';
import { IGridsConfigurationService } from '@common/interfaces';
import { FileUploader } from 'ng2-file-upload';
import { FileUploadResultComponent } from '@common/file-uploader';

@Component({
  selector: 'lxft-daily-audit-spi-rules',
  templateUrl: './daily-audit-spi-rules.component.html'
})
export class DailyAuditSpiRulesComponent extends GridPageCtrl implements OnInit {

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
    protected eventsService: EventsService,
    protected cdr: ChangeDetectorRef,
    protected layoutService: LayoutService,
    @Inject('GridsConfigurationService') private configurationService: IGridsConfigurationService,
    private pageActionsService: ConfigurationService,
    protected rulesApiService: RulesApiService,
    protected rulesService: RulesService,
    private fileService: FileUploadService,
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
    const pageName: string = this.route.snapshot.data.name;
    if (pageName) {
      if (this.setPaginationOptions(pageName)) {
        this.initGridConfigAndData();
        this.initFileUploader();
      }
      this.setPageAvailableActions(pageName);
    }
  }

  setPageAvailableActions(name: string): void {
    const pageName = this.configurationService.getPageActionsConfig(name);
    if (pageName) {
      this.pageActionsService.getPageActions(pageName).subscribe(result => (this.availableActions = result));
    }
  }

  setPaginationOptions(name: string): boolean {
    this.paginationOptions = this.configurationService.getGridConfig(name);
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

  create(row) {
    this.rulesApiService
      .createRule(this.paginationOptions.pathForApi, row)
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
          this.toastr.error(`${error.FieldName}: ${error.Message}`);
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

  // todo: create separate file uploader component
private initFileUploader() {
  this.uploader = this.fileService.createTemplateUploader(`${this.paginationOptions.gridName}`
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
    this.api.downloadRuleTemplate(this.paginationOptions.gridName);
  }
}
