import { FieldOptions, LookupFieldOptionsBuilder } from '@common/fields/models';
import { ListOptionModel } from '@common/fields/models/ListOptionModel';

/**
 * Provides handler for extending data model with client actions property
 */
export class AddAvailableImportersHandler {

  static handler = (row: {
    Ein: string,
    Importer: string,
    AvailableImporters: ListOptionModel[],
    AvailableImporterOptions: FieldOptions
  }): void => {
    if (row.AvailableImporters && row.AvailableImporters.length) {
      const options = new LookupFieldOptionsBuilder()
        .create()
        .options(row.AvailableImporters)
        .build();
      row.AvailableImporterOptions = options;
    }
  }
}
