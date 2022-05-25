import { isNull } from 'util';

/**
 * Provides handler for disabling 'select' for all records with different mapping and filing statuses
 */
export class SameStatusHandler {
  private mappingStatus: number | string = null;
  private filingStatus: number | string = null;

  constructor(private getMappingStatus: (row: any) => number | string, private getFilingStatus: (row: any) => number | string) {
    this.clear();
  }

  private rowIsWrong(row, expectedMappingStatus: string | number = null, expectedFilingStatus: string | number = null) {
    return this.getMappingStatus(row) !== (isNull(expectedMappingStatus) ? this.mappingStatus : expectedMappingStatus) ||
      this.getFilingStatus(row) !== (isNull(expectedFilingStatus) ? this.filingStatus : expectedFilingStatus);
  }

  set(mappingStatus: number, filingStatus: number): void {
    this.mappingStatus = mappingStatus;
    this.filingStatus = filingStatus;
  }

  clear(): void {
    this.mappingStatus = null;
    this.filingStatus = null;
  }

  handler = (row: any): void => {
    if (this.mappingStatus != null && this.filingStatus != null) {
      if (this.rowIsWrong(row)) {
        row.UserActions.Select = false;
      }
    }
  }

  checker = (rows: any[]): boolean => {
    if (rows.length === 0)  { return true; }
    const mappingStatus = this.getMappingStatus(rows[0]);
    const filingStatus = this.getFilingStatus(rows[0]);
    const wrongRow = rows.find(x => this.rowIsWrong(x, mappingStatus, filingStatus));
    if (wrongRow) { return false; } else { return true; }
  }
}
