import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { Filter } from '../models';

@Pipe({
    name: 'basic'
})
@Injectable()
export class BasicFiltersPipe implements PipeTransform {
    transform(items: Filter[], args: any[]): any {
        if (!items) { return []; }
        return items.filter(item => item.advanced !== true);
    }
}
