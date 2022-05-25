import { Pipe, PipeTransform } from '@angular/core';
import { InboundRecordParameter, FieldsFilterSettings } from '@inbound/models';
import { filter } from 'ramda';

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {

  transform(items: InboundRecordParameter[], searchText: string, markedForReviewEnabled: boolean): any[] {
    if (!items) { return []; }
    let result = [...items];
    if (searchText) {
      searchText = searchText.toLowerCase();
      result = result.filter(it => it.title.toLowerCase().indexOf(searchText) > -1);
    }
    if (markedForReviewEnabled) {
      result = result.filter(f => f.markedForFiltering);
    }
    return result;
  }

}
