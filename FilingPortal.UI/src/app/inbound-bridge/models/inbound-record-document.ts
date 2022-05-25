import { FileItem } from 'ng2-file-upload';
import { InboundRecordDocumentStatus } from './inbound-record-document-status';

export class UploadedFile {
  public name: string;
  public fileObj: FileItem;
  public type: string;
  public description: string;
}

export class InboundRecordDocument extends UploadedFile {
  public id: number;
  public filingHeaderId: number;
  public status: InboundRecordDocumentStatus;
  public isManifest: boolean;
}
