import { Injectable } from '@angular/core';

import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

import { ConfirmationComponent } from '@common/confirmation';
import { ConfirmationParameters } from '@common/models/confirmation-parameters';
import { ModalOptions } from '@common/models';

@Injectable()
export class ModalService {

  constructor(private modal: NgbModal) { }

  confirm(data: ConfirmationParameters): Promise<boolean> {
    return new Promise((resolve) => {
      const modalRef = this.modal.open(ConfirmationComponent);
      (<ConfirmationComponent>modalRef.componentInstance).modalInfo = data;
      return modalRef.result.then(() => resolve(true)).catch(() => resolve(false));
    });
  }

  open(component, options?: ModalOptions, skipSettings?: boolean): Promise<any> {
    const modalSettings: NgbModalOptions = {
      backdrop: options && options.isCloseOnBackdrop || 'static',
      keyboard: false,
      size: options && options.isLargeSize ? 'lg' : 'sm',
      windowClass: options && options.windowClass ? options.windowClass : 'modal-m'
    };

    const modalRef = this.modal.open(component, !skipSettings ? modalSettings : undefined);
    modalRef.componentInstance.modalInfo = options;

    return modalRef.result.then(
      data => {
        this.restoreModalOpenClass();
        return data;
      });
  }

  isExistModal() {
    const modals = document.querySelectorAll('.modal');
    return modals.length > 0;
  }

  restoreModalOpenClass() {
    if (this.isExistModal()) {
      const body = document.body;
      body.classList.add('modal-open');
    }
  }
}
