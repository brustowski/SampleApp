import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import * as R from 'ramda';

import { AddVesselFormConfig } from '@common/fields/models';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { VesselImportEditModel } from '@common/models';
import { InboundRecordParameter } from '@inbound/models';
import { FieldsService } from '@common/fields/services/fields.service';
import { AddInboundRecordService } from '@inbound/services';

@Component({
  selector: 'app-add-vessel-modal',
  templateUrl: './add-vessel-modal.component.html'
})
export class AddVesselModalComponent extends ModalCtrl implements OnInit {
  allConfigs: AddVesselFormConfig[] = [];
  index: number;
  defaultConfig: AddVesselFormConfig;
  isExport: boolean;

  get config(): AddVesselFormConfig {
    return this.allConfigs.length ? this.allConfigs[this.index] : null;
  }

  constructor(private vesselService: AddInboundRecordService, protected activeModal: NgbActiveModal, private fieldsService: FieldsService) {
    super(activeModal);
  }

  addConfig(config: AddVesselFormConfig = { fields: null }): void {
    this.index = this.allConfigs.push({ fields: R.clone(config.fields || this.defaultConfig.fields) }) - 1;
  }

  ngOnInit() {
    this.vesselService.getAddFormConfig().subscribe(config => {
      this.defaultConfig = config;
      this.isExport = this.modalInfo.isExport ? true : false;
      this.addConfig();
    });
  }

  save(close: boolean = false) {
    const model = <VesselImportEditModel>{ Id: this.config.id };
    this.config.fields.forEach(x => {
      model[x.name] = x.value;
    });
    this.vesselService.addOrEditRecord(model).subscribe(validationModel => {
      this.vesselService.setValidationErrors(this.config, validationModel);
      if (validationModel.IsValid) {
        if (close) {
          this.activeModal.close();
        } else {
          this.config.id = validationModel.Data;
          this.addConfig(this.config);
        }
      }
    });
  }

  cancel(): void {
    this.allConfigs.length > 1 ? super.ok() : super.cancel();
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
