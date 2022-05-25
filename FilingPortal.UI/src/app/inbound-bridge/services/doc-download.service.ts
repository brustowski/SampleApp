import { Injectable } from '@angular/core';
import { InboundConfigurationService } from './inbound-configuration.service';
import { FilingParametersService } from './filing-parameters.service';
import { convertModelToFormData } from '@app/functions';
import { GeneratorFileNames } from '@common/models';
import { saveAs } from 'file-saver';
import { InboundType, InboundRecordDocument } from '@inbound/models';
import { HttpClient } from '@angular/common/http';
import { InboundTypeWatcherService } from './inbound-type-watcher.service';

class ProcessorMapping {
  constructor(public inboundType: InboundType, public fileName: string, public process: (data?: any) => void) { }
}

@Injectable({
  providedIn: 'root'
})
export class DocDownloadService {

  private processorsMappings: ProcessorMapping[] = [];

  constructor(private mappingsService: InboundConfigurationService,
    private filingParametersService: FilingParametersService,
    private httpClient: HttpClient,
    private inboundTypeWatcher: InboundTypeWatcherService) {

    // Put mappings here
    this.processorsMappings.push(new ProcessorMapping(
      InboundType.Pipeline,
      GeneratorFileNames.PipelineApiCalculatorFileName,
      this.GenerateApiCalculator
    ));

    this.processorsMappings.push(new ProcessorMapping(
      InboundType.ZonesEntry, 'inbound_xml', this.downloadInboundXmlFile));
    this.processorsMappings.push(new ProcessorMapping(
      InboundType.ZonesFtz214, 'inbound_xml', this.downloadInboundXmlFile));
  }

  processDocument(document: InboundRecordDocument): void {
    const mapping = this.getProcessor(document);
    if (mapping) {
      mapping.process.bind(this, document)();
    } else {
      this.downloadDocument(document);
    }
  }

  private downloadDocument(document: InboundRecordDocument): void {
    if (document.id > 0) {
      this.mappingsService.getApiPath().subscribe(path => {
        const url = `${path}/documents/${document.id}`;
        window.open(url, '_blank');
      });
    }
  }

  private getProcessor(document: InboundRecordDocument): ProcessorMapping {
    const mapping = this.processorsMappings.find(x => x.inboundType === this.inboundTypeWatcher.inboundType &&
      x.fileName === document.name);
    return mapping;
  }

  private GenerateApiCalculator(): void {
    const path = this.mappingsService.getMvcPathByType(InboundType.Pipeline);
    const url = `${path}/GenerateApiCalculator`;
    const model = this.filingParametersService.getCurrentModel();
    const sendable = convertModelToFormData(model);
    this.httpClient.post(url, sendable, {
      responseType: 'blob',
    }).subscribe(data => saveAs(data, GeneratorFileNames.PipelineApiCalculatorFileName));

  }

  private downloadInboundXmlFile(document: InboundRecordDocument): void {
    if (document.id > 0) {
      this.mappingsService.getApiPath().subscribe(path => {
        const url = `${path}/documents/inbound/${document.id}`;
        window.open(url, '_blank');
      });
    }
  }
}
