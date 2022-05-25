import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { ConfirmationParameters } from '@common/models/confirmation-parameters';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'lxft-truck-export-filing-confirmation-dialog',
  templateUrl: './truck-export-filing-confirmation-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TruckExportFilingConfirmationDialogComponent extends ModalCtrl {
  public modalInfo: ConfirmationParameters = {text: ''};

  constructor(
    protected activeModal: NgbActiveModal
  ) {
    super(activeModal);
  }

}
