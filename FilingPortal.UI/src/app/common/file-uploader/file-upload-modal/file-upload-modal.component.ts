import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploader, FileItem } from 'ng2-file-upload';
import { SelectSearchSettings } from '@common/fields/models/SelectSearchSettingsModel';
import { FieldsApiService } from '@common/fields/services/fields-api.service';
import { LookupFieldOptions, LookupFieldOptionsBuilder, FieldOptions } from '@common/fields/models';
import { ListOptionModel } from '@common/fields/models/ListOptionModel';
import { FieldLookupComponent } from '@common/fields/field-lookup/field-lookup.component';

@Component({
  selector: 'lxft-file-upload-modal',
  templateUrl: './file-upload-modal.component.html'
})
export class FileUploadModalComponent extends ModalCtrl implements OnInit {

  @ViewChildren('docTypeSelector') docTypeSelectors: QueryList<FieldLookupComponent>;

  public uploader: FileUploader;
  public hasBaseDropZoneOver: boolean = false;
  public documentTypeSettings: FieldOptions;
  public descriptions: {} = {};
  options: ListOptionModel[];
  get usedOptions(): string[] {
    return this.uploader.queue.filter(x => x.formData['docType']).map(x => x.formData['docType']);
  }

  onAfterAddingFile(fileItem: FileItem) {
    fileItem.formData['docTypeSettings'] = { ...this.documentTypeSettings };
    this.updateUsedOptions();
  }

  constructor(protected activeModal: NgbActiveModal, protected fieldsApi: FieldsApiService) {
    super(activeModal);
    this.documentTypeSettings = new LookupFieldOptions(); // prevent server call when user data is loading
  }

  ngOnInit() {
    this.uploader = this.modalInfo.uploader;
    this.uploader.onAfterAddingFile = this.onAfterAddingFile.bind(this);
    this.setDocumentTypeSelection();
  }

  private setDocumentTypeSelection() {
    this.fieldsApi
      .getSelectFieldOptions(this.getDocumentTypeSearchSettings())
      .subscribe(options => {
        this.options = options;
        options.forEach(x => {
          this.descriptions[x.Value] = x['Description'];
        });
        this.setDocumentTypeSettings(options);
      });
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

  onDocTypeChange(item: FileItem, value: string): void {
    item.formData['docType'] = value;
    item.formData['description'] = this.descriptions[value];

    this.updateUsedOptions();
  }

  private updateUsedOptions() {
    this.uploader.queue.forEach(fileItem => {
      const usedOptions = this.usedOptions.filter(x => x !== fileItem.formData['docType']);
      (<LookupFieldOptions>fileItem.formData['docTypeSettings']).options =
        this.options.filter(x => usedOptions.findIndex(y => y === x.Value) === -1);
    });
    this.getDocTypeSelectors().forEach(x => x.refresh());
  }

  protected getDocTypeSelectors(): QueryList<FieldLookupComponent> {
    return this.docTypeSelectors;
  }

  public remove(fileItem: FileItem) {
    fileItem.remove();
    this.updateUsedOptions();
  }
}
