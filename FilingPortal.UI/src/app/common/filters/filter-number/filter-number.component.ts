import { Component, OnInit, Input } from '@angular/core';
import { Filter } from '../models';

@Component({
  selector: 'lxft-filter-number',
  templateUrl: './filter-number.component.html'
})
export class FilterNumberComponent implements OnInit {

  @Input() filter: Filter;

  constructor() { }

  ngOnInit() {
  }

}
