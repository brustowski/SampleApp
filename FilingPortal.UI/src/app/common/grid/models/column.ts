import { TemplateRef } from '@angular/core';

export class Column {
  prop: string = '';
  keyProp: string;
  name: string = '';
  minWidth: number;
  maxWidth: number;
  width: number = 100;
  sortable: boolean = false;
  draggable: boolean = false;
  resizeable: boolean = false;
  searchable: boolean = false;
  isKeyField: boolean = false;
  isVisible: boolean = true;
  isSystem: boolean = false;
  checkboxable: boolean = false;
  canAutoResize: boolean = true;
  headerCheckboxable: boolean = false;
  headerClass: string;
  cellClass: string | Function;
  frozenRight: boolean;
  frozenLeft: boolean;
  isDefaultSort: boolean = false;
  isViewOpen: boolean = false;
  cellTemplate: TemplateRef<any>;
  headerTemplate?: TemplateRef<any>;
  type: string; // 'tags'
  dependOn: string;
  dependOnProperty: string;
  columns: Column[];
  summaryFunc: (cells: any[]) => any;
  summaryTemplate: TemplateRef<any>;
  comparator: (
    valueA: any,
    valueB: any,
    rowA: any,
    rowB: any,
    sortDirection: any
  ) => number;

  get updatedProperty(): string {
    return this.keyProp ? this.keyProp : this.prop;
  }
}
