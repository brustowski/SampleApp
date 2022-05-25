import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { InboundRecordDocument, InboundRecordDocumentStatus, TreeNode } from '@inbound/models';
import { InboundRecordsService, InboundRecordsValidator } from '@inbound/services';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { FieldsApiService } from '@common/fields/services/fields-api.service';
import { ModalService } from '@common/services';
import { SelectSearchSettings } from '@common/fields/models/SelectSearchSettingsModel';
import * as R from 'ramda';
import { LookupFieldOptionsBuilder, FieldOptions } from '@common/fields/models';
import { DocDownloadService } from '@inbound/services/doc-download.service';

@Component({
  selector: 'lxft-filing-parameters-tree-document-tab',
  templateUrl: './filing-parameters-tree-document-tab.component.html'
})
export class FilingParametersTreeDocumentTabComponent implements OnInit {
  @Input() node: TreeNode<InboundRecordDocument>;
  @Input() viewMode: boolean = false;
  @Output() onChange: EventEmitter<any> = new EventEmitter();

  public uploader: FileUploader;
  public hasBaseDropZoneOver: boolean = false;

  public documentTypeSettings: FieldOptions;

  constructor(private docDownloadService: DocDownloadService
    , private service: InboundRecordsService
    , private fieldsApi: FieldsApiService
    , private validator: InboundRecordsValidator
    , private modal: ModalService) {
    this.uploader = this.service.uploader;
  }

  ngOnInit() {
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

  public get undeletedDocuments(): InboundRecordDocument[] {
    return this.node.data.filter(
      d => d.status !== InboundRecordDocumentStatus.Deleted
    );
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
    this.node.data.unshift(doc);
    this.onChange.emit();
  }

  public removeDocument(doc: InboundRecordDocument): void {
    this.modal
      .confirm({ text: 'Are you sure you want to delete the document?' })
      .then(confirmed => {
        if (confirmed) {
          if (doc.status === InboundRecordDocumentStatus.New) {
            doc.fileObj.remove(); // check if it necessary
            const idx = this.node.data.indexOf(doc);
            this.node.data.splice(idx, 1);
          } else {
            doc.status = InboundRecordDocumentStatus.Deleted;
          }
          this.onChange.emit();
        }
      });
  }

  public validateDocumentType(docType): boolean {
    return this.validator.validateDocumentType(docType);
  }

  onTypeChanged(docType: any, doc: InboundRecordDocument) {
    doc.type = docType;
    const option = (R.find(d => d.Value === docType, this.documentTypeSettings.options) as any);
    doc.description = option ? option.Description : '';
    this.markAsUpdated(doc);
    this.onChange.emit();
  }

  private markAsUpdated(doc: InboundRecordDocument): void {
    if (doc.status === InboundRecordDocumentStatus.None) {
      doc.status = InboundRecordDocumentStatus.Updated;
    }
  }

  downloadFile(doc: InboundRecordDocument): void {
    this.docDownloadService.processDocument(doc);
  }
}
