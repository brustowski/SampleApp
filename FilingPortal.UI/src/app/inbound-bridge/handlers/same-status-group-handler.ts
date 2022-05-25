import { isNull } from 'util';
import { MappingStatus } from '@common/models';

/**
 * Provides handler for disabling 'select' for all records with different mapping and filing statuses
 */
export class SameStatusGroupHandler {
  private mappingStatus: number | string = null;
  private filingStatus: number | string = null;

  constructor(private getMappingStatus: (row: any) => number | string, private getFilingStatus: (row: any) => number | string) {
    this.clear();
  }

  private isInOneGroup(row, expectedMappingStatus: string | number = null, expectedFilingStatus: string | number = null): boolean {
    const rowMappingStatus = this.getMappingStatus(row);
    const rowFilingStatus = this.getFilingStatus(row);
    const mappingStatus = isNull(expectedMappingStatus) ? this.mappingStatus : expectedMappingStatus;
    const filingStatus = isNull(expectedFilingStatus) ? this.filingStatus : expectedFilingStatus;

    const statusGroups = [
      // In Review may be selected with Open
      [MappingStatus.Open, MappingStatus.InReview, MappingStatus.Updated]
    ];

    const groupFound = statusGroups.find(group =>
      group.includes(rowMappingStatus as MappingStatus) && group.includes(mappingStatus as MappingStatus));

    if (groupFound) {
      return true;
    }

    if (rowMappingStatus === mappingStatus && rowFilingStatus === filingStatus) {
      return true;
    }

    return false;
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
      if (!this.isInOneGroup(row)) {
        row.UserActions.Select = false;
      }
    }
  }

  checker = (rows: any[]): boolean => {
    if (rows.length === 0) { return true; }
    const mappingStatus = this.getMappingStatus(rows[0]);
    const filingStatus = this.getFilingStatus(rows[0]);
    const wrongRow = rows.find(x => !this.isInOneGroup(x, mappingStatus, filingStatus));
    if (wrongRow) { return false; } else { return true; }
  }
}
