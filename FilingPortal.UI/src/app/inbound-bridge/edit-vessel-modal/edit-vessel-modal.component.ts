import { Component, OnInit } from '@angular/core';
import { AddVesselFormConfig } from '@common/fields/models';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { VesselImportEditModel } from '@common/models';
import { Observable } from 'rxjs';
import { InboundRecordParameter } from '@inbound/models';
import { FieldsService } from '@common/fields/services/fields.service';
import { AddInboundRecordService } from '@inbound/services';

@Component({
  selector: 'app-edit-vessel-modal',
  templateUrl: './edit-vessel-modal.component.html',
})
export class EditVesselModalComponent extends ModalCtrl implements OnInit {

  config: AddVesselFormConfig;
  recordId: number = null;
  isLoaded: boolean = false;
  isExport: boolean;

  constructor(
    private vesselService: AddInboundRecordService,
    protected activeModal: NgbActiveModal,
    private fieldsService: FieldsService
  ) {
    super(activeModal);
  }

  ngOnInit() {
    this.vesselService.getAddFormConfig()
      .subscribe(config => {
        this.config = config;
        this.recordId = this.modalInfo.rowData.Id;
        this.isExport = this.modalInfo.isExport ? true : false;
        this.vesselService.GetRecord(this.recordId).subscribe(vessel => {
          Object.keys(vessel).forEach(x => {
            const field = this.config.fields.find(f => f.name === x);
            if (field) {
              field.value = vessel[x];
            }
          });
          this.isLoaded = true;
        });
      });
  }

  save() {
    const model = <VesselImportEditModel>{ Id: this.recordId };
    this.config.fields.forEach(x => {
      model[x.name] = x.value;
    });
    this.vesselService
      .addOrEditRecord(model)
      .subscribe(validationModel => {
        if (validationModel.IsValid) {
          this.ok();
        } else {
          this.vesselService.setValidationErrors(this.config, validationModel);
        }
      });
  }

  onBlur(field: InboundRecordParameter): void {
    if (this.isExport && (field.name === 'ContactId' || field.name === 'GoodsDescription' || field.name === 'TransportRef')) {
      const description = this.config.fields.find(f => f.name === 'Description');
      if (description) {
        const contact = this.fieldsService.getDisplayValue('ContactId');
        const goodsDescription = this.config.fields.find(f => f.name === 'GoodsDescription');
        const transportRef = this.config.fields.find(f => f.name === 'TransportRef');
        const vals = [];
        if (contact) {
          vals.push(contact);
        }
        if (transportRef && transportRef.value) {
          vals.push(transportRef.value);
        }
        if (goodsDescription && goodsDescription.value) {
          vals.push(goodsDescription.value);
        }
        description.value = vals.join(' - ');
      }
    }
  }

}
