import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { FieldOptions } from '../models/FieldOptions';
import { RequiredField } from '../field-required-ctrl';

@Component({
  selector: 'lxft-field-multiline-text',
  templateUrl: './field-multiline-text.component.html',
  providers: [{ provide: RequiredField, useExisting: forwardRef(() => FieldMultilineTextComponent) }]
})
export class FieldMultilineTextComponent extends RequiredField implements OnInit {
  @Input() options: FieldOptions = new FieldOptions();
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
