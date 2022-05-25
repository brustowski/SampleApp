import { Component, OnInit, Input, Output, EventEmitter, HostBinding } from '@angular/core';
import { InboundRecordParameter } from '@inbound/models';
import { isLookup } from '@common/typeguards/field-options';
import { SelectModel, SelectModelBuilder } from '../models/SelectModel';
import { isNullOrUndefined } from 'util';

@Component({
  selector: 'lxft-fields-selector, [lxft-fields-selector]',
  templateUrl: './fields-selector.component.html'
})
export class FieldsSelectorComponent implements OnInit {
  @Input() field: InboundRecordParameter;
  @Input() viewMode: boolean;

  @Output() valueChange = new EventEmitter<any>();
  @Output() onBlur = new EventEmitter<FocusEvent>();

  @HostBinding('class.no-border')
  @Input() hideBorder: boolean = false;
  @HostBinding('class.data-item-textfield') isSimpleField: boolean = undefined;

  constructor() { }

  ngOnInit() {
    this.isSimpleField = this.field && this.field.type !== 'Complex';
  }

  onChildChange(value: any): void {
    this.valueChange.emit(value);
  }

  getLookupFieldOptions(): SelectModel {
    if (isLookup(this.field.options)) {
      return new SelectModelBuilder()
        .create()
        .options(isLookup(this.field.options) ? this.field.options.options : [])
        .search(this.field.options.searchable)
        .backendOptions(!isNullOrUndefined(this.field.options.provider))
        .providerName(this.field.options.provider)
        .build();
    }
  }

  onInputBlur(event: FocusEvent) {
    this.onBlur.emit(event);
  }
}
