import { Injectable } from '@angular/core';
import { InboundRecordParameter } from '@inbound/models';

import { Column } from '../models/column';
import { ColumnBuilder } from '../models/column-builder';
import { ColumnServer } from '../models/column-server';

@Injectable()
export class GridPageColumnsService {
  constructor() { }

  createColumn() {
    return new ColumnBuilder().create();
  }

  parseGridColumnFromInboundRecordParameter(param: InboundRecordParameter) {
    return this.createColumn()
      .displayName(param.title)
      .fieldName(param.name)
      .sortable(true)
      .defaultSort(false)
      .type(param.type)
      .minWidth(200)
      .comparator(this.inboundRecordParameterComparator.bind(this))
      .build();
  }

  parseGridColumn(column: ColumnServer): Column {
    return this.createColumn()
      .displayName(column.DisplayName)
      .fieldName(column.FieldName)
      .keyFieldName(column.KeyFieldName)
      .sortable(column.IsSortable)
      .defaultSort(column.DefaultSorted)
      .isViewOpen(column.IsViewOpen)
      .maxWidth(column.MaxWidth)
      .minWidth(column.MinWidth)
      .align(column.Align)
      .type(column.EditType)
      .dependOn(column.DependOn)
      .dependOnProperty(column.DependOnProperty)
      .resizable(column.IsResizable)
      .searchable(column.IsSearchable)
      .keyField(column.IsKeyField)
      .subColumns(this.getSubColums(column))
      .build();
  }

  getSubColums(column: ColumnServer): Column[] {
    if (column.EditType === 'composite' && column.Columns) {
      return column.Columns.map(x => this.parseGridColumn(x));
    }
    return null;
  }

  getActionColumn(tml): Column {
    return this.createColumn()
      .displayName('Actions')
      .fieldName('actions')
      .system()
      .sortable(false)
      .frozenRight(true)
      .cellTemplate(tml)
      .minWidth(111)
      .maxWidth(111)
      .align(2)
      .build();
  }

  getStatusColumn(tml): Column {
    return this.createColumn()
      .displayName('Filing Status')
      .fieldName('status')
      .sortable(false)
      .frozenRight(true)
      .cellTemplate(tml)
      .minWidth(105)
      .maxWidth(105)
      .build();
  }

  getErrorsColumn(tml) {
    return this.createColumn()
      .displayName('Issues')
      .fieldName('errors')
      .sortable(false)
      .frozenRight(true)
      .cellTemplate(tml)
      .align(2)
      .minWidth(80)
      .build();
  }

  setCheckboxColumn() {
    return this.createColumn()
      .fieldName('checkbox')
      .system()
      .sortable(false)
      .headerCheckboxable(true)
      .checkboxable(true)
      .align(2)
      .minWidth(45)
      .maxWidth(45)
      .build();
  }

  getNotificationColumn(tml) {
    return this.createColumn()
      .fieldName('notification')
      .displayName('&nbsp;')
      .system()
      .sortable(false)
      .frozenLeft(true)
      .cellTemplate(tml)
      .align(2)
      .minWidth(40)
      .maxWidth(40)
      .width(40)
      .build();
  }

  inboundRecordParameterComparator(param1: InboundRecordParameter, param2: InboundRecordParameter) {
    let value1 = param1.value;
    let value2 = param2.value;
    if (param1.type === param2.type) {
      switch (param1.type) {
        case 'FloatNumber':
        case 'Number': {
          value1 = Number.parseFloat(value1);
          value2 = Number.parseFloat(value2);
        }
      }
    }
    if (value1 < value2) {
        return -1;
      }
      if (value1 > value2) {
        return 1;
      }
  }
}
