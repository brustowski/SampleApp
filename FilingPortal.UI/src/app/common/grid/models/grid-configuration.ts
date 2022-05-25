import { Sort } from './page';
import { ColumnConfiguration } from './column-configuration';

export class GridConfiguration {
  pageSize: number;
  sorting: Sort;
  columns: ColumnConfiguration[];
}
