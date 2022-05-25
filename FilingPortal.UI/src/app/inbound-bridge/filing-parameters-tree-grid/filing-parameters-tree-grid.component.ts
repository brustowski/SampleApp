import { Component, OnInit, Input, ViewChild, TemplateRef } from '@angular/core';
import { InboundRecordParameter, TreeNode } from '@inbound/models';
import { FieldBlurEvent } from '@common/fields/models';
import { FilingParametersTreeNodeComponent } from '@inbound/filing-parameters-tree-node';
import { FilingParametersService } from '@inbound/services';
import { Column } from '@common/grid/models';
import { GridPageColumnsService } from '@common/grid/services';
import { SortType } from '@custom/ngx-datatable';

@Component({
  selector: 'lxft-filing-parameters-tree-grid',
  templateUrl: './filing-parameters-tree-grid.component.html',
})
export class FilingParametersTreeGridComponent extends FilingParametersTreeNodeComponent implements OnInit {
  constructor(protected filingService: FilingParametersService,
    private columnsService: GridPageColumnsService) {
    super(filingService);
  }
  @ViewChild('actionsTmpl') actionsTmpl: TemplateRef<any>;
  @ViewChild('editableColumnTmpl') editableColumnTmpl: TemplateRef<any>;
  @ViewChild('hdrTpl') hdrTpl: TemplateRef<any>;

  _node: TreeNode<InboundRecordParameter>;
  columns: Column[] = [];
  rows: any[] = [];
  defSort: { prop: string, dir: 'asc' | 'desc' }[] = null;

  SortType = SortType;

  get filingHeaderId(): number {
    return this.filingService.getCurrentFilingHeaderId();
  }

  @Input()
  get node(): TreeNode<InboundRecordParameter> {
    return this._node;
  }
  set node(value: TreeNode<InboundRecordParameter>) {
    const columns: Column[] = [];

    this._node = value;
    value.children.forEach(child => {
      const data = child.getData();
      const row = {};
      data.forEach(d => {
        let column = columns.find(x => x.prop === d.name);
        if (!column) {
          column = this.columnsService.parseGridColumnFromInboundRecordParameter(d);
          column.cellTemplate = this.editableColumnTmpl;
          columns.push(column);
        }
        row[d.name] = d;
      });
      row['ViewMode'] = this.viewMode;
      row['node'] = child;
      this.rows.push(row);

      this.defSort = [{ prop: columns[0].prop, dir: 'asc' }];
    });

    this.columns = columns;
    this.setColumnTmplts();
    this.setActionsColumn();
  }

  setColumnTmplts(): void {
    this.columns.forEach(column => {
      column.cellTemplate = this.editableColumnTmpl;
    });
  }

  setActionsColumn() {
    if (!this.viewMode) {
      const actionsColumn = this.columnsService.getActionColumn(this.actionsTmpl);
      actionsColumn.headerTemplate = this.hdrTpl;
      this.columns.push(actionsColumn);
    }
  }

  ngOnInit() {
    super.ngOnInit();
  }

  public getRowId(row: { id: number }): number {
    return row.id;
  }

  getConfig(row: { config: InboundRecordParameter[] }, prop: string): InboundRecordParameter {
    return row.config.find(x => x.name === prop);
  }

  onBlur(fieldInfo: InboundRecordParameter, event: FieldBlurEvent): void {
    if (event.oldValue === undefined || event.oldValue !== fieldInfo.value) {
      this.onFieldBlur(fieldInfo);
    }
  }

  remove(row: { node: TreeNode<InboundRecordParameter> }, $event: any) {
    const index = this.rows.indexOf(row);

    this.delete(row.node.name, row.node.id);

    if (index > -1) {
      const newRows = [...this.rows];
      newRows.splice(index, 1);
      this.rows = newRows;
    }
    this.onNodeChange($event);
  }

  success(): void {
    this.filingService.refresh(this.node.name, this.node.id);
    this.onResize.emit();
  }
}
