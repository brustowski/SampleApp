import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { LoaderService, FileUploadService, ModalService } from '@common/services';
import { locationPath } from '@app/utils';
import {
  FileUploadValidationResultComponent
} from '../file-upload-validation-result/file-upload-validation-result.component';

@Component({
  selector: 'lxft-import-with-confirmation-button',
  templateUrl: './import-with-confirmation-button.component.html'
})
export class ImportWithConfirmationButtonComponent implements OnInit {

  public uploader: FileUploader;

  @ViewChild('fileInput')
  public fileInput: ElementRef;
  @Input()
  public url: string;
  @Input()
  public title: string;
  @Input()
  public gridName: string;
  @Input()
  public btnClass: string = 'btn btn-primary';
  @Input()
  public iconClass: string;
  @Output()
  public success = new EventEmitter<any>();
  @Output()
  public error = new EventEmitter<any>();

  constructor(private loader: LoaderService,
    private fileService: FileUploadService,
    private modalService: ModalService) { }

  ngOnInit() {
    this.uploader = new FileUploader(<FileUploaderOptions>{
      url: `${locationPath}/${this.url}`,
      method: 'POST'
    });

    this.uploader.onCompleteAll = () => {
      this.fileInput.nativeElement.value = '';
    };

    this.uploader.onAfterAddingFile = file => {
      this.loader.showLoader();
      file.upload();
      file.onSuccess = (fileId: string) => {
        this.loader.hideLoader();
        this.fileService.checkUploadedFile(fileId, this.gridName)
          .switchMap(result => this.modalService.open(FileUploadValidationResultComponent,
            { uploadResult: result, windowClass: 'message-box' }))
          .switchMap(() => this.fileService.importUploadedFile(fileId, this.gridName))
          .finally(() => {
            this.fileService.deleteUploadedFile(fileId).subscribe();
          })
          .subscribe(() => this.success.emit());
      };
      file.onError = (response: string, status: number) => {
        this.loader.hideLoader();
        let message = 'An error occurred during import process';
        if (status === 400 || status === 403) {
          const jsonResponse = JSON.parse(response);
          message = jsonResponse.Message;
        }
        this.error.emit(message);
      };
    };
  }

}
