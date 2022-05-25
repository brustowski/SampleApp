import { Column } from './column';

export class ColumnConfiguration {
  prop: string;
  name: string;
  isVisible: boolean;
  width: number;

  static fromColumn(column: Column): ColumnConfiguration {
    return {
      prop: column.prop,
      name: column.name,
      isVisible: column.isVisible,
      width: column.width
    };
  }
}
