import { Component, Input, Output, EventEmitter } from '@angular/core';
import { v4 } from 'uuid';

import { FieldOptions } from '../models/FieldOptions';

@Component({
  selector: 'lxft-field-confirmation',
  templateUrl: './field-confirmation.component.html'
})
export class FieldConfirmationComponent {
  @Input() options: FieldOptions = new FieldOptions();
  @Input() value: boolean = false;
  @Output() valueChange = new EventEmitter<any>();
  @Input() viewMode: boolean = false;

  id = v4();

  constructor() { }

  onChange() {
    this.value = !this.value;
    this.valueChange.emit(this.value ? 1 : 0);
  }
}
