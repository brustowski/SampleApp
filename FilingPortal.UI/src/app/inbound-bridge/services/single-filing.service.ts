import { Injectable } from '@angular/core';

import { of } from 'rxjs/observable/of';
import { Observable } from 'rxjs/Observable';

import { InboundRecordsApiService } from './inbound-records-api.service';

import { SingleFilingModel } from '@common/models/single-filing-model';
import { InboundRecordFileModel } from '@inbound/models';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class SingleFilingService {
  private models: { [filingHeaderId: number]: SingleFilingModel } = {};

  public get filingHeaderIds(): number[] {
    return Object.keys(this.models).map(x => +x);
  }
  public set filingHeaderIds(ids: number[]) {
    this.models = {};
    ids.forEach(x => (this.models[x] = null));
  }

  constructor(
    private apiService: InboundRecordsApiService,
    private toastr: ToastrService
  ) {}

  clear(): void {
    this.models = {};
  }

  saveIntermediateResult(): Observable<any> {
    const models = this.createModels();

    return this.apiService.save({ Models: models });
  }

  startFiling(): Observable<number[]> {
    const models = this.createModels();
    return this.apiService.file({ Models: models }).map(result => {
      result.Messages.forEach(m => this.toastr.error(m));
      return result.Results.filter(r => !r.IsValid)
        .map(r => r.FilingHeaderId);
    });
  }

  cancelFilingProcess(): Observable<void> {
    return this.apiService.undo(this.filingHeaderIds);
  }

  GetModelByFilingHeader(rowId: number): SingleFilingModel {
    return this.models[rowId];
  }

  SetModel(filingHeaderId: number, model: SingleFilingModel): void {
    this.models[filingHeaderId] = model;
  }

  createModels(): InboundRecordFileModel[] {
    const modelsCopy = { ...this.models };

    const results: InboundRecordFileModel[] = [];

    for (const filingId of Object.keys(modelsCopy)) {
      const nFilingId = +filingId;
      if (!isNaN(nFilingId)) {
        if (modelsCopy[nFilingId]) {
          results.push(modelsCopy[nFilingId].model);
        } else {
          const untouchedRecord = new InboundRecordFileModel();
          untouchedRecord.FilingHeaderId = nFilingId;
          results.push(untouchedRecord);
        }
      }
    }

    return results;
  }

  public getRecordIds(): Observable<number[]> {
    return this.apiService.getRecordIds(this.filingHeaderIds);
  }
}
