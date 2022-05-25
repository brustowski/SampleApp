import {Component, OnInit, Input, EventEmitter, Output} from '@angular/core';

@Component({
  selector: 'lxft-grid-selection-toolbar',
  templateUrl: './grid-selection-toolbar.component.html'
})
export class GridSelectionToolbarComponent implements OnInit {
  @Input() selectedRows: any[];
  @Input() selectAllEnabled: boolean = false;
  @Output() onClear: EventEmitter<any>;

  constructor() {
    this.onClear = new EventEmitter<any>();
  }

  ngOnInit() {
  }

  clearSelected() {
    this.onClear.emit();
  }
}
