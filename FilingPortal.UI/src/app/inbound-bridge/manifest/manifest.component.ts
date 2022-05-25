import { Component, OnInit } from '@angular/core';

import 'rxjs/add/operator/finally';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { InboundRecordsService } from '@inbound/services';

import { ModalCtrl } from '@common/modal/modal-ctrl';
import { Manifest } from '@common/fields/models/manifest';

@Component({
  selector: 'lxft-manifest',
  templateUrl: './manifest.component.html'
})
export class ManifestComponent extends ModalCtrl implements OnInit {
  public manifest: Manifest;
  public isLoading: boolean;
  public parsedView: boolean = true;

  constructor(
    private inboundRecordsService: InboundRecordsService,
    protected activeModal: NgbActiveModal,
  ) {
    super(activeModal);
  }

  ngOnInit() {
    const manifestId: number = +this.modalInfo.manifestId;
    this.inboundRecordsService.getManifest(manifestId)
      .subscribe(manifest => {
        this.manifest = manifest;
      });
  }
}
