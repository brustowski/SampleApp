import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { Filter } from '../models';

@Pipe({
  name: 'advanced'
})
@Injectable()
export class AdvancedFilterPipe implements PipeTransform {
  transform(items: Filter[], args: any[]): any {
    if (!items) { return []; }
    return items.filter(item => item.advanced === true);
  }
}
