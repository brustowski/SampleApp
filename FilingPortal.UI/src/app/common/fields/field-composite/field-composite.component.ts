import { Component, Input, Output, EventEmitter } from '@angular/core';

import { CompositeFieldOptions } from '../models';

@Component({
  selector: 'lxft-field-composite',
  templateUrl: './field-composite.component.html'
})
export class FieldCompositeComponent {

  @Input() value: any;
  @Output() valueChange = new EventEmitter<any>();
  @Input() options: CompositeFieldOptions;

  constructor() {
  }

  setValue(fieldName: string, value: any) {
    this.value[fieldName] = value;
    this.valueChange.emit(this.value);
  }

}
