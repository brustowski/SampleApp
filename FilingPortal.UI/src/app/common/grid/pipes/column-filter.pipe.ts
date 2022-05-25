import { Pipe, PipeTransform } from '@angular/core';
import { Column } from '@common/grid/models/column';
import { ColumnConfiguration } from '../models';

@Pipe({
  name: 'columnFilter'
})
export class ColumnFilterPipe implements PipeTransform {

  transform(items: Column[] | ColumnConfiguration[], searchText: string): any[] {
    if (!items) { return []; }
    if (!searchText) { return items; }
    searchText = searchText.toLowerCase();
    return items.filter(it => it.name.toLowerCase().indexOf(searchText) > -1);
  }

}
