import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AddVesselFormConfig } from '@common/fields/models';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { IsfInboundEditModel } from '@common/models';
import { AddInboundRecordService } from '@inbound/services';
import { dateFormatCompact, dateFormatFull } from '@app/utils';
import * as moment from 'moment/moment';
import { ToastrService } from 'ngx-toastr';
import * as R from 'ramda';
import { FieldsService } from '@common/fields/services/fields.service';

@Component({
  selector: 'lxft-isf-add-inbound-modal',
  templateUrl: './isf-add-inbound-modal.component.html'
})
export class IsfAddInboundModalComponent extends ModalCtrl implements OnInit {
  allConfigs: AddVesselFormConfig[] = [];
  index: number;
  defaultConfig: AddVesselFormConfig;

  isLoaded: boolean = false;
  viewMode: boolean = false;

  importerFieldName = 'ImporterId';
  consigneeFieldName = 'ConsigneeId';
  buyerFieldName = 'BuyerId';
  billsFieldName = 'Bills';
  containersFieldName = 'Containers';

  get config(): AddVesselFormConfig {
    return this.allConfigs.length ? this.allConfigs[this.index] : null;
  }

  constructor(private addRecordService: AddInboundRecordService, protected activeModal: NgbActiveModal, private toastr: ToastrService,
    private fieldService: FieldsService) {
    super(activeModal);
  }

  ngOnInit() {
    this.viewMode = this.modalInfo.viewMode || false;

    this.addRecordService.getAddFormConfig().subscribe(config => {
      this.defaultConfig = config;
      this.addConfig();
      if (this.modalInfo && this.modalInfo.rowData && this.modalInfo.rowData.Id) {
        this.config.id = this.modalInfo.rowData.Id;

        this.addRecordService.GetRecord(this.config.id).subscribe(record => {
          Object.keys(record).forEach(x => {
            if (record[x]) {
              const field = this.config.fields.find(f => f.name === x);
              if (field) {
                if (field.type === 'Date') {
                  const m = moment(record[x]);
                  field.value = m.format(dateFormatFull.toUpperCase());
                } else {
                  field.value = record[x];
                }
              }
            }
          });
          this.isLoaded = true;
        });
      } else {
        this.isLoaded = true;
      }
    });
  }

  addConfig(config: AddVesselFormConfig = { fields: null }): void {
    this.index = this.allConfigs.push({ fields: R.clone(config.fields || this.defaultConfig.fields) }) - 1;
  }

  save(close: boolean, gotoFiling: boolean) {
    const model = <IsfInboundEditModel>{ Id: this.config.id };
    this.config.fields.forEach(x => {
      model[x.name] = x.value;
    });
    this.addRecordService.addOrEditRecord(model).subscribe(validationModel => {
      this.addRecordService.setValidationErrors(this.config, validationModel);
      if (validationModel.IsValid) {
        if (close) {
          const ids = this.allConfigs.filter(x => x.id).map(x => x.id);
          ids.push(validationModel.Data);
          this.activeModal.close({ ids: ids, gotoFiling });
        } else {
          this.config.id = validationModel.Data;
          this.addConfig(this.config);
          this.clone();
        }
      }
    });
  }

  clone() {
    delete (this.config.id);

    const billsField = this.config.fields.find(x => x.name === this.billsFieldName);
    const containersField = this.config.fields.find(x => x.name === this.containersFieldName);

    billsField.value = [];
    containersField.value = [];

    this.viewMode = false;
  }

  onImporterChanged() {
    const value = this.config.fields.find(x => x.name === this.importerFieldName).value;

    const consigneeField = this.config.fields.find(x => x.name === this.consigneeFieldName);
    const buyerField = this.config.fields.find(x => x.name === this.buyerFieldName);

    consigneeField.value = value;
    buyerField.value = value;

    this.fieldService.setFieldValue(consigneeField.options.name, value);
    this.fieldService.setFieldValue(buyerField.options.name, value);
  }
}
