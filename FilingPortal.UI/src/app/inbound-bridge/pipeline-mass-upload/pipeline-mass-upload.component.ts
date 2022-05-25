import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FieldsApiService } from '@common/fields/services/fields-api.service';
import { FileUploadModalComponent } from '@common/file-uploader';
import { FileItem } from 'ng2-file-upload';
import { FieldLookupComponent } from '@common/fields/field-lookup/field-lookup.component';
import * as R from 'ramda';

enum DocTypeMappingResult {
  None,
  Single,
  Multiple,
  All
}

@Component({
  selector: 'app-pipeline-mass-upload',
  templateUrl: './pipeline-mass-upload.component.html'
})
export class PipelineMassUploadComponent extends FileUploadModalComponent implements OnInit {

  @ViewChildren('docTypeSelector') docTypeSelectors: QueryList<FieldLookupComponent>;

  get usedOptions(): string[] {
    const options = this.uploader.queue.filter(x => x.formData['docType']).map(x => x.formData['docType']);
    return R.without(['DKT', 'COR'], options);
  }

  batchCodes: string[];

  constructor(protected activeModal: NgbActiveModal, protected fieldsApi: FieldsApiService) {
    super(activeModal, fieldsApi);
  }

  ngOnInit() {
    super.ngOnInit();
    this.setBatchCodes();
  }

  onAfterAddingFile(fileItem: FileItem) {
    super.onAfterAddingFile(fileItem);

    const extension = this.getFileExtension(fileItem.file.name);
    let docType = null;
    switch (extension) {
      case 'pdf': if (this.countAffectedRecords(fileItem) > 0) { docType = 'DKT'; } break;
      case 'eml': if (this.countAffectedRecords(fileItem) > 0) { docType = 'COR'; } break;
      case 'msg': if (this.countAffectedRecords(fileItem) > 0) { docType = 'COR'; } break;
    }

    if (docType) {
      this.onDocTypeChange(fileItem, docType);
    }
  }

  /*
  * Counts records where this file will be uploaded
  */
  countAffectedRecords(fileItem: FileItem): number {
    return this.batchCodes.filter(x => fileItem.file.name.toLowerCase().indexOf(x) >= 0).length;
  }

  /*
  * Gets file extension by file name
  */
  getFileExtension(filename: string): string {
    return (filename || '').split('.').pop().toLowerCase();
  }

  /*
  * Sets up list of selected batch codes
  */
  setBatchCodes() {
    this.batchCodes = this.modalInfo.batchCodes.map(x => x.toLowerCase());
  }

  onDocTypeChange(item: FileItem, value: string): void {
    super.onDocTypeChange(item, value);
    this.updateAffectedRecords(item, value);
  }

  updateAffectedRecords(item: FileItem, docType: string) {
    if (docType === 'DKT' || docType === 'COR') {
      const affectedRecordsAmount = this.countAffectedRecords(item);
      item.formData['records'] = affectedRecordsAmount === 0 ?
        DocTypeMappingResult.None :
        affectedRecordsAmount === 1 ? DocTypeMappingResult.Single : DocTypeMappingResult.Multiple;
    } else {
      item.formData['records'] = DocTypeMappingResult.All;
    }
  }
}
