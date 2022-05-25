import { Component, Input, Output, EventEmitter } from '@angular/core';
import { v4 } from 'uuid';

import { FieldOptions } from '../models/FieldOptions';

@Component({
  selector: 'lxft-field-boolean',
  templateUrl: './field-boolean.component.html'
})
export class FieldBooleanComponent {
  boolValue: boolean = false;

  @Input() options: FieldOptions = new FieldOptions();
  @Input()
  get value(): '0' | '1' {
    return this.boolValue ? '1' : '0';
  }
  set value(v: '0' | '1') {
    if (v) {
      this.boolValue = Boolean(JSON.parse(v));
    } else {
      this.boolValue = false;
    }
  }
  @Output() valueChange = new EventEmitter<any>();
  @Input() viewMode: boolean = false;

  id = v4();

  constructor() { }

  onChange() {
    this.valueChange.emit(this.value);
  }
}
