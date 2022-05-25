import { Component, OnInit, Input } from '@angular/core';
import { ModalCtrl } from '../../modal/modal-ctrl';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUploadResultModel } from '../../models/file-upload-result-model';
import { saveAs } from 'file-saver';
import { FileUploadResults } from '@common/models';

@Component({
  selector: 'lxft-file-upload-result',
  templateUrl: './file-upload-result.component.html'
})
export class FileUploadResultComponent extends ModalCtrl implements OnInit {
  @Input() modalInfo: FileUploadResults;

  models: {errorsCount: number, serverModel: FileUploadResultModel }[];

  private lineDelimiter = '\r\n';
  private columnDelimiter = '\t';

  constructor(protected activeModal: NgbActiveModal) {
    super(activeModal);
  }

  ngOnInit() {
    this.models = [];

    this.modalInfo.models.forEach(model => {
      let count = 0;
      if (model.commonErrors) {
        count += model.commonErrors.length;
      }
      if (model.parsingErrors) {
        count += model.parsingErrors.length;
      }
      if (model.validationErrors) {
        count += model.validationErrors.length;
      }

      this.models.push({serverModel: model, errorsCount: count});
    });
  }

  download(model: FileUploadResultModel): void {
    const filebody = this.convertToCsv(model);
    const blob: Blob = new Blob([filebody], { type: 'text/csv' });
    const fileName = `${model.fileName}.log`;
    saveAs(blob, fileName);
  }

  private convertToCsv(model: FileUploadResultModel): string {
    const fileBody: string[] = [];
    fileBody.push(`File Name: ${model.fileName}`);
    fileBody.push(`Uploaded Records: ${model.count}`);
    if (model.commonErrors) {
      fileBody.push(`Common Errors: ${model.commonErrors.length}`);
      model.commonErrors.forEach(e => fileBody.push(e));
    }
    if (model.parsingErrors) {
      fileBody.push(`Parsing Errors: ${model.parsingErrors.length}`);
      if (model.parsingErrors.length) {
        const properties: string[] = Object.getOwnPropertyNames(model.parsingErrors[0]);
        fileBody.push(properties.map(x => this.normalizeTitle(x)).join(this.columnDelimiter));
        model.parsingErrors.forEach(e => fileBody.push(properties.map(p => e[p]).join(this.columnDelimiter)));
      }
    }
    if (model.validationErrors) {
      fileBody.push(`Validation Errors: ${model.validationErrors.length}`);
      if (model.validationErrors.length) {
        const properties: string[] = Object.getOwnPropertyNames(model.validationErrors[0]);
        fileBody.push(properties.map(x => this.normalizeTitle(x)).join(this.columnDelimiter));
        model.validationErrors.forEach(e => fileBody.push(properties.map(p => e[p]).join(this.columnDelimiter)));
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
