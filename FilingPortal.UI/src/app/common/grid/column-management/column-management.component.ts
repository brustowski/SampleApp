import { Component, OnInit } from '@angular/core';

import * as R from 'ramda';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ModalCtrl } from '@common/modal/modal-ctrl';
import { ColumnConfiguration, ColumnManagementConfiguration } from '../models';

@Component({
  selector: 'lxft-column-management',
  templateUrl: './column-management.component.html'
})
export class ColumnManagementComponent extends ModalCtrl implements OnInit {

  public modalInfo: { configuration: ColumnConfiguration[] };
  public available: ColumnManagementConfiguration[];
  public visible: ColumnManagementConfiguration[];

  constructor(protected activeModal: NgbActiveModal) {
    super(activeModal);
  }

  ngOnInit() {
    this.reset();
  }

  reset(): void {
    this.available = [...this.modalInfo.configuration.filter(x => !x.isVisible).map(x => ({ ...x, isSelected: false }))];
    this.visible = [...this.modalInfo.configuration.filter(x => x.isVisible).map(x => ({ ...x, isSelected: false }))];
  }

  ok(): void {
    super.ok([...this.available, ...this.visible]);
  }

  isSelected(items: ColumnManagementConfiguration[]): boolean {
    return items.some(x => x.isSelected);
  }

  select(item: ColumnManagementConfiguration): void {
    item.isSelected = !item.isSelected;
  }

  makeVisible(): void {
    this.visible.push(...this.available.filter(x => x.isSelected).map(x => ({ ...x, isVisible: true, isSelected: false })));
    this.available = this.available.filter(x => !x.isSelected);
  }

  makeVisibleAll(): void {
    if (this.available.length) {
      this.visible.push(...this.available.map(x => ({ ...x, isVisible: true, isSelected: false })));
      this.available = [];
    }
  }

  makeHidden(): void {
    this.available.push(...this.visible.filter(x => x.isSelected).map(x => ({ ...x, isVisible: false, isSelected: false })));
    this.visible = this.visible.filter(x => !x.isSelected);
  }

  makeHiddenAll(): void {
    if (this.visible.length) {
      this.available.push(...this.visible.map(x => ({ ...x, isVisible: false, isSelected: false })));
      this.visible = [];
    }
  }
}
