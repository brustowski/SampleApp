export class SingleSelectHandler {
  private selectedRecordId: number | string;

  constructor(private getRowId: (row: any) => number | string) {
  }

  set(selectedRecordId: number | string): void {
    this.selectedRecordId = selectedRecordId;
  }

  clear(): void {
    this.selectedRecordId = undefined;
  }

  handler = (row: any): void => {
    row.UserActions.Select = this.selectedRecordId ? this.getRowId(row) === this.selectedRecordId : true;
  }

}
