import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'lxft-column-field-selector',
  templateUrl: './column-field-selector.component.html'
})
export class ColumnFieldSelectorComponent {

  @Input() value: any;
  @Output() valueChange = new EventEmitter<any>();
  @Input() options: any;
  @Input() type: string;

  constructor() {
  }

  get childValue(): any {
    return this.value;
  }
  set childValue(value: any) {
    this.value = value;
    this.valueChange.emit(value);
  }
}
