import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { InboundRecordParameter, FieldsFilterSettings } from '@inbound/models';
import { FieldBlurEvent } from '@common/fields/models';

@Component({
  selector: 'lxft-filing-parameters-tree-node-fields',
  templateUrl: './filing-parameters-tree-node-fields.component.html'
})
export class FilingParametersTreeNodeFieldsComponent implements OnInit {
  @Input() data: InboundRecordParameter[] = [];
  @Input() filterSettings: FieldsFilterSettings;
  @Input() viewMode: boolean = false;
  @Output() onChange: EventEmitter<any> = new EventEmitter();
  @Output() onFieldBlur: EventEmitter<InboundRecordParameter> = new EventEmitter();

  constructor() { }

  ngOnInit() { }

  onFieldChange($event): void {
    this.onChange.emit($event);
  }

  onBlur(fieldInfo: InboundRecordParameter, event: FieldBlurEvent): void {
    if (event.oldValue === undefined || event.oldValue !== fieldInfo.value) {
      this.onFieldBlur.emit(fieldInfo);
    }
  }
}
