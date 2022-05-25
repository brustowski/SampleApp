import { Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

export class ModalCtrl {
  @Input() modalInfo: any;

  constructor(protected activeModal: NgbActiveModal) { }

  ok(result?: any) {
    this.activeModal.close(result);
  }

  cancel() {
    this.activeModal.dismiss();
  }
}
