import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReviewSectionExportModel, ReviewSectionField } from '@common/models';
import { InboundRecordParameter, TreeNode } from '@inbound/models';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { Observable } from 'rxjs/Observable';
import { InboundConfigurationService, InboundTypeWatcherService } from '.';

@Injectable({
  providedIn: 'root'
})
export class TreeNodeGridImportService {

  constructor(private mappingsService: InboundConfigurationService
    , private httpService: HttpClient
    , private inboundTypeWatcherService: InboundTypeWatcherService) { }

  getUploader(nodes: TreeNode<InboundRecordParameter>[], filingHeaderId: number): FileUploader {
    if (nodes.length) {
      const workflow = this.inboundTypeWatcherService.inboundType;
      const sectionName = nodes[0].name;
      const parentId = nodes[0].parentId;
      const uploader = new FileUploader(<FileUploaderOptions>{
        url: this.mappingsService.getReviewGridImportPath(),
        method: 'POST',
        parametersBeforeFiles: true,
        removeAfterUpload: true,
        additionalParameter: {
          section: sectionName,
          parentId: parentId,
          filingHeaderId: filingHeaderId,
          workflow: workflow
        }
      });
      return uploader;
    }
  }

  export(nodes: TreeNode<InboundRecordParameter>[]): Observable<{ name: string, file: Blob }> {
    const result = <ReviewSectionExportModel>{ Values: [] };
    if (nodes.length) {
      result.SectionName = nodes[0].name;
      result.Columns = nodes[0].data.map(x => <ReviewSectionField>{ Id: x.id, Value: x.title });
      nodes.forEach(node => {
        result.Values.push(node.data.map(x => <ReviewSectionField>{ Id: x.id, Value: x.value }));
      });
      return this.httpService.post(this.mappingsService.getReviewGridExportPath(), result, { responseType: 'blob' }).map(file => {
        return {
          name: nodes[0].name,
          file: file
        };
      }
      );
    }
  }
}
