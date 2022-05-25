import { HighlightingType } from '@common/models';

/**
 * Provides handler for disabling 'select' for all records and highlighting records with same 'Filing Header Id'
 */
export class SameFilingHeaderHandler {
  private filingHeaders: string[];
  private selectedIds: any[];

  constructor(private getRowId: (row: any) => any, private getFilingHeaderId: (row: any) => any) {
    this.clear();
  }

  set(filingHeaderIds: string[], selectedIds: any[]): void {
    this.filingHeaders = filingHeaderIds;
    this.selectedIds = selectedIds;
  }

  clear(): void {
    this.filingHeaders = [];
    this.selectedIds = [];
  }

  handler = (row: any): void => {
    const filingHeaderId = this.getFilingHeaderId(row);
    if (filingHeaderId && this.filingHeaders.findIndex(x => x === filingHeaderId) > -1) {
      if (this.selectedIds.findIndex(x => x === this.getRowId(row)) === -1) {
        row.UserActions.Select = false;
        row.UserHighlightingType = HighlightingType.Success;
        row.Errors = ['Part of consolidated filing'];
      }
    } else if (HighlightingType.Success === row.UserHighlightingType) {
      row.UserHighlightingType = HighlightingType.NoHighlighting;
    }
  }
}
