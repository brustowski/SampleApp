import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Column } from '../models';
import { ColumnConfiguration } from '../models/column-configuration';

import { ModalService } from '@common/services';
import { ColumnManagementComponent } from '../column-management/column-management.component';

@Component({
  selector: 'lxft-column-management-button',
  templateUrl: './column-management-button.component.html'
})
export class ColumnManagementButtonComponent {
  @Input() set columns(columns: Column[]) {
    if (columns) {
      this.configuration = columns.filter(x => !x.isSystem).map(ColumnConfiguration.fromColumn);
    }
  }
  @Output() onApply: EventEmitter<ColumnConfiguration[]> = new EventEmitter();

  private configuration: ColumnConfiguration[] = [];

  constructor(protected modal: ModalService) { }

  open() {
    this.modal.open(ColumnManagementComponent, {
      configuration: this.configuration
      , windowClass: 'column-settings-modal'
    }).then(
      x => {
        this.onApply.emit(x);
      }
    );
  }
}
