import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { InboundRecordParameter, TreeNode } from '@inbound/models';
import { TreeNodeGridImportService } from '@inbound/services/tree-node-grid-import.service';
import { FileItem, FileUploader } from 'ng2-file-upload';
import * as fileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { LoaderService, ModalService } from '@common/services';
import { FileUploadDetailedResultComponent } from '@common/file-uploader';
import { FileUploadDetailedResultModel } from '@common/models';
import { from } from 'rxjs';

@Component({
  selector: 'lxft-filing-parameters-tree-grid-import',
  templateUrl: './filing-parameters-tree-grid-import.component.html',
})
export class FilingParametersTreeGridImportComponent implements OnInit {

  @Input()
  public node: TreeNode<InboundRecordParameter> = null;
  @Input()
  public viewMode: boolean = false;
  @Input()
  public filingHeaderId: number;
  @Output()
  public success = new EventEmitter<any>();
  @Output()
  public error = new EventEmitter<void>();

  public uploader: FileUploader;

  constructor(private treeNodeGridImportService: TreeNodeGridImportService
    , private toastr: ToastrService
    , private loader: LoaderService
    , protected modal: ModalService) { }

  ngOnInit() {
    this.uploader = this.treeNodeGridImportService.getUploader(this.node.children, this.filingHeaderId);
    this.uploader.onAfterAddingFile = (file: FileItem) => {
      this.loader.showLoader();
      file.upload();
      file.onSuccess = (response: string) => {
        this.loader.hideLoader();
        const result = FileUploadDetailedResultModel.Parse(response);
        from(this.modal.open(FileUploadDetailedResultComponent,
          { uploadResult: result, windowClass: 'message-box' }))
          .subscribe(_ => this.success.emit());
      };
      file.onError = (response: string, status: number) => {
        this.loader.hideLoader();
        let message = 'An error occurred during import process';
        if (status === 400 || status === 403) {
          const jsonResponse = JSON.parse(response);
          message = jsonResponse.Message;
        }
        this.toastr.error(message);
        this.error.emit();
      };
    };
  }

  export() {
    this.treeNodeGridImportService.export(this.node.children).subscribe((result) => {
      fileSaver.saveAs(result.file, `${result.name}.xlsx`);
    });
  }
}
