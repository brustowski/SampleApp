import { Component, OnInit } from '@angular/core';
import { RailInboundEditModel } from '@common/models';
import { InboundRecordParameter } from '@inbound/models';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FieldsService } from '@common/fields/services/fields.service';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { FormConfig } from '@common/fields/models';
import { AddInboundRecordService } from '@inbound/services';

@Component({
  selector: 'app-add-manifest-modal',
  templateUrl: './add-manifest-modal.component.html'
})
export class AddManifestModalComponent extends ModalCtrl implements OnInit {

  config: FormConfig;
  recordId: number = null;
  isLoaded: boolean = false;

  constructor(
    private railService: AddInboundRecordService,
    protected activeModal: NgbActiveModal,
    private fieldsService: FieldsService
  ) {
    super(activeModal);
  }

  ngOnInit() {
    this.railService.getAddFormConfig()
      .subscribe(config => {
        this.config = config;
        if (this.modalInfo.rowData && this.modalInfo.rowData.Id) {
          this.recordId = this.modalInfo.rowData.Id;
          this.railService.GetRecord(this.recordId).subscribe(record => {
            Object.keys(record).forEach(x => {
              const field = this.config.fields.find(f => f.name === x);
              if (field) {
                field.value = record[x];
              }
            });
            this.isLoaded = true;
          });
        } else {
          this.isLoaded = true;
        }
      });
  }

  save() {
    const model = <RailInboundEditModel>{ Id: this.recordId };
    this.config.fields.forEach(x => {
      model[x.name] = x.value;
    });
    this.railService
      .addOrEditRecord(model)
      .subscribe(validationModel => {
        if (validationModel.IsValid) {
          this.ok();
        } else {
          this.railService.setValidationErrors(this.config, validationModel);
        }
      });
  }

}
