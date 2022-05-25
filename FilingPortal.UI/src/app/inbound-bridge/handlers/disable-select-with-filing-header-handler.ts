/**
 * Provides handler for disabling 'select' of all records with filling header not equal to: 0 or null or undefined
 */
export class DisableSelectWithFilingHeaderHandler {
  private isDisabled: boolean = false;

  enable(): void {
    this.isDisabled = false;
  }

  disable(): void {
    this.isDisabled = true;
  }

  handler = (row: any): void => {
    if (this.isDisabled && row.FilingHeaderId) {
        row.UserActions.Select = false;
    }
  }
}
