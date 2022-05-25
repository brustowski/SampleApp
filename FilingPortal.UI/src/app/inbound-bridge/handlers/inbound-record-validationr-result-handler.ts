import * as R from 'ramda';
import { InboundRecordErrorViewModel, InboundRecordValidationResultViewModel } from '@inbound/models';
import { HighlightingType } from '@common/models';

export class InboundRecordValidationResultHandler {
  private result: InboundRecordErrorViewModel[] = [];
  private toRestore: number[] = [];

  set validationResult(result: InboundRecordValidationResultViewModel) {
    if (result.IsValid) {
      this.clear();
    } else {
      this.toRestore = R.map<any, any>(x => x.Id, R.difference(this.result, result.RecordErrors));
      this.result = result.RecordErrors;
    }
  }

  clear(): void {
    this.toRestore = this.result.map(res => res.Id);
    this.result = [];
  }

  handler = (row: any): void => {
    if (this.toRestore && this.toRestore.indexOf(row.Id) > -1) {
      if (row.UserHighlightingType === HighlightingType.Warning) {
        row.UserHighlightingType = HighlightingType.NoHighlighting;
        row.Errors = [];
      }
    }
    if (this.result && this.result.length) {
      const val = this.result.find(record => record.Id === row.Id);
      if (val && val.Errors && val.Errors.length) {
        row.Errors = val.Errors;
        row.UserHighlightingType = HighlightingType.Warning;
      }
    }
  }
}
