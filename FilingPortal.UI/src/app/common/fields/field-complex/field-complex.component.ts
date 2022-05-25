import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { InboundRecordParameter } from '@inbound/models';

@Component({
  selector: 'lxft-field-complex',
  templateUrl: './field-complex.component.html',
})
export class FieldComplexComponent implements OnInit {

  @Input() field: InboundRecordParameter;
  @Input() viewMode: boolean;

  @Output() valueChange = new EventEmitter<any>();
  @Output() onBlur = new EventEmitter<any>();

  main: InboundRecordParameter;
  paired: InboundRecordParameter;

  constructor() { }

  ngOnInit() {
    this.main = this.field.additionalFields[0];
    this.paired = this.field.additionalFields[1];
  }

  onChildChange(value: any): void {
    this.valueChange.emit(value);
  }

  onInputBlur(event: any) {
    this.onBlur.emit(event);
  }

}
