import { Component, OnInit, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalCtrl } from '@common/modal/modal-ctrl';
import { saveAs } from 'file-saver';

import { FileUploadDetailedResultModel } from '@common/models/file-upload-detailed-result-model';

@Component({
  selector: 'lxft-file-upload-detailed-result',
  templateUrl: './file-upload-detailed-result.component.html'
})
export class FileUploadDetailedResultComponent extends ModalCtrl implements OnInit {
  @Input() modalInfo: { uploadResult: FileUploadDetailedResultModel };

  uploadResult: FileUploadDetailedResultModel;
  errorsCount: number = 0;
  isContinueButtonVisible: boolean;

  private lineDelimiter = '\r\n';
  private columnDelimiter = '\t';

  constructor(protected activeModal: NgbActiveModal) {
    super(activeModal);
  }

  ngOnInit() {
    this.uploadResult = this.modalInfo.uploadResult;
    let count = 0;
    if (this.uploadResult.commonErrors) {
      count += this.uploadResult.commonErrors.length;
    }
    if (this.uploadResult.parsingErrors) {
      count += this.uploadResult.parsingErrors.length;
    }
    if (this.uploadResult.validationErrors) {
      count += this.uploadResult.validationErrors.length;
    }
    this.errorsCount = count;
    this.isContinueButtonVisible = this.uploadResult.inserted + this.uploadResult.updated > 0;
  }

  download(): void {
    const filebody = this.convertToCsv();
    const blob: Blob = new Blob([filebody], { type: 'text/csv' });
    const fileName = `${this.uploadResult.fileName}.log`;
    saveAs(blob, fileName);
  }

  private convertToCsv(): string {
    const fileBody: string[] = [];
    fileBody.push(`File Name: ${this.uploadResult.fileName}`);
    fileBody.push(`Total Records: ${this.uploadResult.count}`);
    fileBody.push(`Inserted Records: ${this.uploadResult.inserted}`);
    fileBody.push(`Updated Records: ${this.uploadResult.updated}`);
    if (this.uploadResult.commonErrors) {
      fileBody.push(`Common Errors: ${this.uploadResult.commonErrors.length}`);
      this.uploadResult.commonErrors.forEach(e => fileBody.push(e));
    }
    if (this.uploadResult.parsingErrors) {
      fileBody.push(`Parsing Errors: ${this.uploadResult.parsingErrors.length}`);
      if (this.uploadResult.parsingErrors.length) {
        const properties: string[] = Object.getOwnPropertyNames(this.uploadResult.parsingErrors[0]);
        fileBody.push(properties.map(x => this.normalizeTitle(x)).join(this.columnDelimiter));
        this.uploadResult.parsingErrors.forEach(e => fileBody.push(properties.map(p => e[p]).join(this.columnDelimiter)));
      }
    }
    if (this.uploadResult.validationErrors) {
      fileBody.push(`Validation Errors: ${this.uploadResult.validationErrors.length}`);
      if (this.uploadResult.validationErrors.length) {
        const properties: string[] = Object.getOwnPropertyNames(this.uploadResult.validationErrors[0]);
        fileBody.push(properties.map(x => this.normalizeTitle(x)).join(this.columnDelimiter));
        this.uploadResult.validationErrors.forEach(e => fileBody.push(properties.map(p => e[p]).join(this.columnDelimiter)));
      }
    }
    return fileBody.join(this.lineDelimiter);
  }

  private normalizeTitle(title: string): string {
    if (!title) {
      return title;
    }
    let normalized = title[0].toUpperCase();
    for (let i = 1; i < title.length; i++) {
      if (!this.isLower(title[i])) {
        normalized += ' ';
      }
      normalized += title[i];
    }
    return normalized;
  }

  private isLower(char: string): boolean {
    return char === char.toLowerCase();
  }

}
