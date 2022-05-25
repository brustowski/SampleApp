import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';

import { locationPath } from '../../utils';
import { FileUploadResultServerModel } from '../models/file-upload-result-server-model';
import { FileUploadResultModel, ExcelFileValidationErrorServer, FileProcessingDetailedResultViewModelServer,
  FileUploadResults } from '../models';
import { FileUploadDetailedResultModel } from '@common/models/file-upload-detailed-result-model';

@Injectable()
export class FileUploadService {

  constructor(private http: HttpClient) { }

  createFileUploader(url: string): FileUploader {
    return new FileUploader(<FileUploaderOptions>{
      url: `${locationPath}/${url}`,
      method: 'POST'
    });
  }

  createTemplateUploader(gridName: string): FileUploader {
    return new FileUploader({
      url: `${locationPath}/imports/upload/grids/${gridName}`,
      method: 'POST'
    });
  }

  parseFileUploadResultResponse(response: string): FileUploadResults {
    const results: FileUploadResults = new FileUploadResults();
    results.windowClass = 'message-box';

    const parsedObject = JSON.parse(response);
    if (Array.isArray(parsedObject)) {
      parsedObject.forEach(x => {
        const serverModel = x as FileUploadResultServerModel;
        const model = this.mapToResultModel(serverModel);
        results.models.push(model);
      });
    } else {
      const serverModel = JSON.parse(response) as FileUploadResultServerModel;
      const model = this.mapToResultModel(serverModel);
      results.models.push(model);
    }

    return results;
  }

  private mapToResultModel(serverModel: FileUploadResultServerModel) {
    const model = new FileUploadResultModel();
    model.commonErrors = serverModel.CommonErrors;
    model.count = serverModel.Count;
    model.fileName = serverModel.FileName;
    model.validationErrors = serverModel.ValidationErrors;
    model.parsingErrors = serverModel.ParsingErrors;
    model.windowClass = 'message-box';
    return model;
  }

  checkUploadedFile(fileId: string, gridName: string): Observable<FileUploadDetailedResultModel> {
    const url = `${locationPath}/imports/uploads/${fileId}/${gridName}`;
    return this.http
      .get<FileProcessingDetailedResultViewModelServer<ExcelFileValidationErrorServer>>(url)
      .map(serverModel => FileUploadDetailedResultModel.Create(serverModel));
  }

  importUploadedFile(fileId: string, gridName: string): Observable<FileUploadDetailedResultModel> {
    const url = `${locationPath}/imports/uploads/${fileId}/${gridName}`;
    return this.http
      .post<FileProcessingDetailedResultViewModelServer<ExcelFileValidationErrorServer>>(url, {})
      .map(serverModel => FileUploadDetailedResultModel.Create(serverModel));
  }

  deleteUploadedFile(fileId: string): Observable<void> {
    const url = `${locationPath}/imports/uploads/${fileId}`;
    return this.http.delete<void>(url);
  }
}
