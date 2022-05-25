import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { ISubscription } from 'rxjs/Subscription';
import { InboundRecordsService } from '@inbound/services';

import { InboundRecordDocument } from '@inbound/models';
import { DocDownloadService } from '@inbound/services/doc-download.service';

@Component({
  selector: 'lxft-documents-table-section',
  templateUrl: './documents-table-section.component.html'
})
export class DocumentsTableSectionComponent implements OnInit, OnDestroy {

  @Output() onResize: EventEmitter<void> = new EventEmitter();

  private subscription: ISubscription;
  public documents: InboundRecordDocument[];

  constructor(
    private service: InboundRecordsService
    , private docDownloadService: DocDownloadService
  ) { }

  ngOnInit() {
    this.subscription = this.service.documents.subscribe(document => { this.documents = document; this.onResize.emit(); });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  downloadFile(doc: InboundRecordDocument): void {
    this.docDownloadService.processDocument(doc);
  }
}
