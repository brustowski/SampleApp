import { FileUploadResultModel } from './file-upload-result-model';
import { ExcelFileValidationErrorServer, FileProcessingDetailedResultViewModelServer } from './models';

export class FileUploadDetailedResultModel extends FileUploadResultModel {
  inserted: number;
  updated: number;

  static Create(serverModel: FileProcessingDetailedResultViewModelServer<any>): FileUploadDetailedResultModel {
    const model = new FileUploadDetailedResultModel();
    model.commonErrors = serverModel.CommonErrors;
    model.count = serverModel.Count;
    model.fileName = serverModel.FileName;
    model.validationErrors = serverModel.ValidationErrors;
    model.parsingErrors = serverModel.ParsingErrors;
    model.inserted = serverModel.Inserted;
    model.updated = serverModel.Updated;
    return model;
  }

  static Parse(response: string): FileUploadDetailedResultModel {
    const parsed: FileProcessingDetailedResultViewModelServer<ExcelFileValidationErrorServer> = JSON.parse(response);
    return FileUploadDetailedResultModel.Create(parsed);
  }
}
