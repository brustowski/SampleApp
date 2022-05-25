import { Column } from '../models';
import {
  LookupFieldOptionsBuilder,
  LookupFieldSource,
  SourceType,
  FieldType,
  FieldOptionsBuilder,
  CompositeFieldOptionsBuilder
} from '@common/fields/models';

/**
 * Provides handler for extending data model with options property
 */
export class AddFieldOptionsToModelHandler {
  private columns: Array<Column> = [];
  private girdName: string;

  /**
   * Create an instance of heandler
   */
  constructor() { }

  /**
   * Sets columns to add options
   */
  set Columns(columns: Column[]) {
    this.columns = columns;
  }

  set GridName(name: string) {
    this.girdName = name;
  }
  /**
   * handler to do work on row
   */
  handler = (row: any): void => {
    row.options = {};
    this.columns.forEach(column => {
      row.options[column.prop] = this.convertToRowOptions(column);
    });
  }

  convertToRowOptions(column: Column): any {
    let options: any;
    switch (column.type) {
      case 'lookup':
        options = this.convertToLookupFieldOptions(column);
        break;
      case 'composite':
        options = this.convertToCompositeOptions(column);
        break;
      default:
        options = this.convertToFieldOptions(column);
        break;
    }
    return options;

  }

  private convertToLookupFieldOptions(column: Column) {
    const builder = new LookupFieldOptionsBuilder();
    builder
      .create()
      .name(column.updatedProperty)
      .title(column.name)
      .fieldType(FieldType.Lookup)
      .source(new LookupFieldSource(this.girdName, SourceType.Grid, false, column.prop))
      .searchable(column.searchable)
      .dependsOn(column.dependOn);
    return builder.build();
  }

  private convertToCompositeOptions(column: Column): any {
    const builder = new CompositeFieldOptionsBuilder();
    builder.create()
      .name(column.prop)
      .title(column.name)
      .fieldType(FieldType.Composite);
    column.columns.forEach(x => builder.addOption(this.convertToRowOptions(x)));
    return builder.build();
  }

  private convertToFieldOptions(column: Column) {
    const builder = new FieldOptionsBuilder();
    builder
      .create()
      .name(column.prop)
      .title(column.name);
    switch (column.type) {
      case 'integer':
        builder.fieldType(FieldType.Integer);
        break;
      case 'float':
        builder.fieldType(FieldType.Float);
        break;
      case 'boolean':
        builder.fieldType(FieldType.Boolean);
        break;
      default:
        builder.fieldType(FieldType.Text);
        break;
    }
    return builder.build();
  }
}
