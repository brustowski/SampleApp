import { Component, Input } from '@angular/core';
import { Filter } from '../models';

@Component({
  selector: 'lxft-filter-float-number',
  templateUrl: './filter-float-number.component.html'
})
export class FilterFloatNumberComponent {

  @Input() filter: Filter;

  constructor() { }

}
