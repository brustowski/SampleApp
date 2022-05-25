import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalCtrl } from '../modal/modal-ctrl';
import { ConfirmationParameters } from '@common/models/confirmation-parameters';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html'
})
export class ConfirmationComponent extends ModalCtrl implements OnInit {
  public modalInfo: ConfirmationParameters = {text: ''};

  constructor(
    protected activeModal: NgbActiveModal
  ) {
    super(activeModal);
  }

  ngOnInit() { }
}
