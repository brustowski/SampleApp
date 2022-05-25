import { InboundRecordField } from '@common/models';
import { ImportRecordRowServer } from './import-record-row-server';
export class RecordFieldToRowConverter {
  private collection: ImportRecordRowServer[] = [];

  private createItem(filingHeaderId: number): ImportRecordRowServer {
    const item = <ImportRecordRowServer>{};
    item.FilingHeaderId = filingHeaderId;
    item.RecordId = this.collection.length + 1;
    item.Parameters = [];
    this.collection.push(item);
    return item;
  }

  private getItem(filingHeaderId: number): ImportRecordRowServer {
    if (!isNaN(filingHeaderId)) {
      let item = this.collection.find(x => x.FilingHeaderId === filingHeaderId);
      if (!item) {
        item = this.createItem(filingHeaderId);
      }
      return item;
    }
    return <ImportRecordRowServer>{ Parameters: [] };
  }
  public addValue(model: InboundRecordField): void {
    const item = this.getItem(model.FilingHeaderId);
    item.Parameters.push(model);
  }
  public convert(): ImportRecordRowServer[] {
    return this.collection;
  }
}
