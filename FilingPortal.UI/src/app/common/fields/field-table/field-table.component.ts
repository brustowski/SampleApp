import { Component, OnInit, Input, forwardRef, ViewChild, TemplateRef, ViewChildren } from '@angular/core';
import { RequiredField } from '../field-required-ctrl';
import { FieldOptions } from '../models';
import { InboundRecordParameter } from '@inbound/models';
import { Column, ColumnBuilder } from '@common/grid/models';
import { ColumnMode, DatatableComponent } from '@custom/ngx-datatable';
import { Guid } from 'guid-typescript';

class Config {
  fields: InboundRecordParameter[];
  uniqueId: Guid;
}

@Component({
  selector: 'lxft-field-table',
  templateUrl: './field-table.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldTableComponent) }]
})
export class FieldTableComponent extends RequiredField implements OnInit {
  private _grid: DatatableComponent = null;
  private _config: InboundRecordParameter[];
  private columnBuilder: ColumnBuilder = new ColumnBuilder();

  @Input()
  get config() {
    return this._config;
  }
  set config(value: InboundRecordParameter[]) {
    if (value) {
      this._config = value;
      this.refreshConfig();
      this.valueChange.emit([]);
    }
  }

  private _innerValue: any;

  @Input() get value(): any {
    return this._innerValue;
  }
  set value(value: any) {
    if (value !== this._innerValue) {
      this._innerValue = value;
      this.processNewValue();
    }
  }

  @ViewChild('editableColumnTmpl')
  editableColumnTmpl: TemplateRef<any>;
  @ViewChild('hdrTpl') hdrTpl: TemplateRef<any>;
  @ViewChild('actionsTmpl') actionsTmpl: TemplateRef<any>;

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
  columns: Column[] = [];
  rows: { config: Config }[] = [];

  constructor() {
    super();
  }

  ColumnMode = ColumnMode;

  ngOnInit() {
    this.processNewValue();
  }

  onGridRendered(grid: DatatableComponent) {
  }

  setColumnTmplts(): void {
    this.columns.forEach(column => {
      column.cellTemplate = this.editableColumnTmpl;
    });
  }

  addActionsColumn() {
    if (this.columns.findIndex(x => x.name === 'actions') === -1) {
      const actionsColumn = this.columnBuilder
        .create()
        .fieldName('actions')
        .maxWidth(83)
        .minWidth(83)
        .headerTemplate(this.hdrTpl)
        .cellTemplate(this.actionsTmpl)
        .build();
      this.columns.push(actionsColumn);
    }
  }

  refreshConfig() {
    if (this.config) {
      this.columns = this.config.map(x => this.columnBuilder.create().fieldName(x.name).displayName(x.title).type(x.type).build());
      this.setColumnTmplts();
      if (!this.viewMode) {
        this.addActionsColumn();
      }
    }
  }

  add(rowProcessFunction: (field: InboundRecordParameter, configField: InboundRecordParameter) => void = null): { config: Config, ViewMode: boolean } {
    const newRow = { config: <Config>{ uniqueId: Guid.create(), fields: [] }, ViewMode: false };
    this.config.forEach(x => {
      const fieldClone: InboundRecordParameter = JSON.parse(JSON.stringify(x));
      fieldClone.name = `${x.name}_${newRow.config.uniqueId.toString()}`;
      fieldClone.options.name = `${x.options.name}_${newRow.config.uniqueId.toString()}`;
      if (fieldClone.options.dependsOn) {
        fieldClone.options.dependsOn = `${x.options.dependsOn}_${newRow.config.uniqueId.toString()}`;
      }
      if (rowProcessFunction) {
        rowProcessFunction(fieldClone, x);
      }
      newRow.config.fields.push(fieldClone);
    });
    this.rows.push(newRow);
    this.rows = [...this.rows];
    this.onValueChange();
    return newRow;
  }

  remove(row: any) {
    const index = this.rows.indexOf(row);
    if (index > -1) {
      const newRows = [...this.rows];
      newRows.splice(index, 1);
      this.rows = newRows;
    }
    this.onValueChange();
  }

  copy(row: { config: Config }): { config: Config, ViewMode: boolean } {
    const setFieldValue = (field: InboundRecordParameter, configField: InboundRecordParameter) => {
      const originalRowName = `${configField.name}_${row.config.uniqueId.toString()}`;
      field.value = row.config.fields.find(x => x.name === originalRowName).value;
    };

    return this.add(setFieldValue);
  }

  getConfig(row: { config: Config }, prop: string): InboundRecordParameter {
    return row.config.fields.find(x => x.name === `${prop}_${row.config.uniqueId.toString()}`);
  }

  getRowId(row: { config: Config }): string {
    return row.config.uniqueId.toString();
  }

  onValueChange() {
    const values = [];
    this.rows.forEach(row => {
      const rowValue = {};
      this.config.forEach(field => {
        const value = row.config.fields.find(x => x.name === `${field.name}_${row.config.uniqueId.toString()}`).value;
        rowValue[field.name] = value;
      });
      values.push(rowValue);
    });
    this._innerValue = values;
    this.valueChange.emit(values);
  }

  onViewModeChanged() {
    this.refreshConfig();
  }

  processNewValue() {
    this.rows.forEach(x => this.remove(x));
    this.checkRequired(this.value);
    if (this.value instanceof Array && this.config) {
      this.value.forEach(row => {
        const newRow = this.add();
        newRow.config.fields.forEach(field => {
          const fieldName = field.name.substr(0, field.name.indexOf(`_${newRow.config.uniqueId.toString()}`));
          field.value = row[fieldName];
        });
      });
      this.onValueChange();
    }
  }
}
