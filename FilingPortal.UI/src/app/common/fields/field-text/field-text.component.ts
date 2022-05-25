import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FieldOptions } from '../models/FieldOptions';
import { RequiredField } from '../field-required-ctrl';

@Component({
  selector: 'lxft-field-text',
  templateUrl: './field-text.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldTextComponent) }]
})
export class FieldTextComponent extends RequiredField implements OnInit {
  @Input() value: any = null;

  constructor() {
    super();
  }

  ngOnInit() {
    if (this.viewMode) {
      return;
    }
    this.checkRequired(this.value);
  }
}
