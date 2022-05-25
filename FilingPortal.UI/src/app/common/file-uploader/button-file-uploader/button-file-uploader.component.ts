import { Component, OnInit, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';

import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';

import { locationPath } from '@app/utils';
import { LoaderService } from '@common/services';

@Component({
  selector: 'lxft-button-file-uploader',
  templateUrl: './button-file-uploader.component.html'
})
export class ButtonFileUploaderComponent implements OnInit {

  public uploader: FileUploader;

  @ViewChild('fileInput')
  public fileInput: ElementRef;
  @Input()
  public url: string;
  @Input()
  public title: string;
  @Input()
  public btnClass: string = 'btn btn-primary';
  @Input()
  public iconClass: string;
  @Input()
  public disabled: boolean = false;
  @Output()
  public success = new EventEmitter<any>();
  @Output()
  public error = new EventEmitter<any>();

  constructor(private loader: LoaderService) { }

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
      file.onSuccess = (response: string) => {
        this.loader.hideLoader();
        this.success.emit(response);
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
