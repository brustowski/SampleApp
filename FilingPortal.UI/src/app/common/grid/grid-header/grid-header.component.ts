import { Component, Input } from '@angular/core';
import { Column } from '../models';

@Component({
  selector: 'lxft-grid-header, [lxft-grid-header]',
  templateUrl: './grid-header.component.html',
})
export class GridHeaderComponent {

  @Input()
  public column: Column = null;

  @Input()
  public sort: () => void;

  constructor() { }

}
