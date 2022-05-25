import { Component, OnInit, OnDestroy } from '@angular/core';

import { ISubscription } from 'rxjs/Subscription';
import * as R from 'ramda';

import { FileUploader, FileItem } from 'ng2-file-upload';

import {
  InboundRecordsService,
  InboundRecordsApiService,
  InboundRecordsValidator
} from '@inbound/services';
import { FieldsApiService } from '@common/fields/services/fields-api.service';
import { ModalService } from '@common/services/modal.service';

import {
  InboundRecordDocument,
  InboundRecordDocumentStatus
} from '@inbound/models';
import { SelectSearchSettings } from '@common/fields/models/SelectSearchSettingsModel';
import { FieldOptions, LookupFieldOptionsBuilder } from '@common/fields/models';
import { DocDownloadService } from '@inbound/services/doc-download.service';

@Component({
  selector: 'lxft-documents-section',
  templateUrl: './documents-section.component.html'
})
export class DocumentsSectionComponent implements OnInit, OnDestroy {
  private subscription: ISubscription;

  public uploader: FileUploader;
  public hasBaseDropZoneOver: boolean = false;
  public documents: InboundRecordDocument[];

  public documentTypeSettings: FieldOptions;

  public get undeletedDocuments(): InboundRecordDocument[] {
    return this.documents.filter(
      d => d.status !== InboundRecordDocumentStatus.Deleted
    );
  }

  constructor(
    private service: InboundRecordsService,
    private apiService: InboundRecordsApiService,
    private validator: InboundRecordsValidator,
    private fieldsApi: FieldsApiService,
    private modal: ModalService,
    private docDownloadService: DocDownloadService
  ) {
    this.uploader = this.service.uploader;
  }

  ngOnInit() {
    this.subscription = this.service.documents.subscribe(
      documents => (this.documents = documents)
    );

    this.uploader.onAfterAddingFile = file => {
      this.addFile(file);
    };
    this.setDocumentTypeSelection();
  }

  private setDocumentTypeSelection() {
    this.fieldsApi
      .getSelectFieldOptions(this.getDocumentTypeSearchSettings())
      .subscribe(options => {
        this.setDocumentTypeSettings(options);
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  private setDocumentTypeSettings(options) {
    this.documentTypeSettings = new LookupFieldOptionsBuilder()
      .create()
      .options(options)
      .searchable(true)
      .build();
  }

  private getDocumentTypeSearchSettings(): SelectSearchSettings {
    const settings = new SelectSearchSettings();
    settings.dataProviderName = 'DocumentTypeDataProvider';
    settings.limit = 2000; // should be enough for documents types
    return settings;
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public addFile(file: FileItem) {
    const doc = new InboundRecordDocument();
    doc.fileObj = file;
    doc.name = file.file.name;
    doc.status = InboundRecordDocumentStatus.New;
    this.documents.unshift(doc);
  }

  public removeDocument(doc: InboundRecordDocument): void {
    this.modal
      .confirm({ text: 'Are you sure you want to delete the document?' })
      .then(confirmed => {
        if (confirmed) {
          if (doc.status === InboundRecordDocumentStatus.New) {
            doc.fileObj.remove(); // check if it necessary
            const idx = this.documents.indexOf(doc);
            this.documents.splice(idx, 1);
          } else {
            doc.status = InboundRecordDocumentStatus.Deleted;
          }
        }
      });
  }

  public validateDocumentType(docType: string): boolean {
    return this.validator.validateDocumentType(docType);
  }

  onTypeChanged(docType: any, doc: InboundRecordDocument) {
    doc.type = docType;
    const option = (R.find(d => d.Value === docType, this.documentTypeSettings.options) as any);
    doc.description = option ? option.Description : '';
    this.markAsUpdated(doc);
  }

  markAsUpdated(doc: InboundRecordDocument): void {
    if (doc.status === InboundRecordDocumentStatus.None) {
      doc.status = InboundRecordDocumentStatus.Updated;
    }
  }

  downloadFile(doc: InboundRecordDocument): void {
    this.docDownloadService.processDocument(doc);
  }
}
