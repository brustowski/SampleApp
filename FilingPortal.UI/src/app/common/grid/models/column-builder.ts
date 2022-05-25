import { Column } from './column';
import { TemplateRef } from '@angular/core';

export class ColumnBuilder {
  model: Column;

  constructor() {}

  create() {
    this.model = new Column();
    return this;
  }

  fieldName(name: string = '') {
    this.model.prop = name;
    return this;
  }

  keyFieldName(name: string) {
    this.model.keyProp = name;
    return this;
  }

  displayName(displayName: string = '') {
    this.model.name = displayName;
    return this;
  }

  maxWidth(maxWidth: number) {
    this.model.maxWidth = maxWidth > 0 ? maxWidth : undefined;

    if (this.model.maxWidth && this.model.maxWidth < this.model.width) {
      this.model.width = this.model.maxWidth;
    }

    return this;
  }

  minWidth(minWidth: number) {
    this.model.minWidth = minWidth > 0 ? minWidth : undefined;

    if (this.model.minWidth) {
      this.model.width = this.model.minWidth;
    }

    return this;
  }

  width(width: number = 150) {
    this.model.width = width;
    return this;
  }

  cellClass(cellClass: string = '') {
    this.model.cellClass = cellClass;
    return this;
  }

  headerClass(headerClass: string = '') {
    this.model.headerClass = headerClass;
    return this;
  }

  sortable(sortable: boolean = true) {
    this.model.sortable = sortable;
    return this;
  }

  resizable(resizable: boolean = true) {
    this.model.resizeable = resizable;
    return this;
  }
  searchable(searchable: boolean = true) {
    this.model.searchable = searchable;
    return this;
  }

  keyField(value: boolean = true) {
    this.model.isKeyField = value;
    return this;
  }

  align(align: number = 1) {
    if (align === 2) {
      this.cellClass('a-center');
      this.headerClass('a-center');
    }
    if (align === 3) {
      this.cellClass('a-right');
      this.headerClass('a-right');
    }
    return this;
  }

  frozenRight(frozenRight: boolean = false) {
    this.model.frozenRight = frozenRight;
    return this;
  }

  frozenLeft(frozenLeft: boolean = false) {
    this.model.frozenLeft = frozenLeft;
    return this;
  }

  cellTemplate(cellTemplate: TemplateRef<any>) {
    this.model.cellTemplate = cellTemplate;
    return this;
  }

  defaultSort(isDefaultSort: boolean = true) {
    this.model.isDefaultSort = isDefaultSort;
    return this;
  }

  isViewOpen(isViewOpen: boolean = false) {
    this.model.isViewOpen = isViewOpen;
    return this;
  }

  checkboxable(checkboxable: boolean = false) {
    this.model.checkboxable = checkboxable;
    return this;
  }

  headerCheckboxable(headerCheckboxable: boolean = false) {
    this.model.headerCheckboxable = headerCheckboxable;
    return this;
  }

  headerTemplate(tmpl: TemplateRef<any> ) {
    this.model.headerTemplate = tmpl;
    return this;
  }

  type(fieldType: string = 'text') {
    this.model.type = fieldType;
    return this;
  }

  dependOn(fieldName: string) {
    this.model.dependOn = fieldName;
    return this;
  }

  dependOnProperty(propertyName: string) {
    this.model.dependOnProperty = propertyName;
    return this;
  }

  comparator(func: (valueA: any, valueB: any, rowA: any, rowB: any, sortDirection: any) => number) {
    this.model.comparator = func;
    return this;
  }

  summaryFunc(func: (cells: any[]) => any) {
    this.model.summaryFunc = func;
    return this;
  }

  summaryTemplate(tmplt: TemplateRef<any>) {
    this.model.summaryTemplate = tmplt;
    return this;
  }

  subColumns(columns: Column[]) {
    this.model.columns = columns;
    return this;
  }

  system(isSystem: boolean = true) {
    this.model.isSystem = isSystem;
    return this;
  }

  build() {
    return this.model;
  }
}
