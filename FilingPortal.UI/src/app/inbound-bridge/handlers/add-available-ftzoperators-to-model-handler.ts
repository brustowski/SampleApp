import { FieldOptions, LookupFieldOptionsBuilder } from '@common/fields/models';
import { ListOptionModel } from '@common/fields/models/ListOptionModel';

/**
 * Provides handler for extending data model with client actions property
 */
export class AddAvailableFtzOperatorsHandler {

  static handler = (row: {
    Ein: string,
    Importer: string,
    AvailableFtzOperators: ListOptionModel[],
    AvailableFtzOperatorsOptions: FieldOptions
  }): void => {
    if (row.AvailableFtzOperators && row.AvailableFtzOperators.length) {
      const options = new LookupFieldOptionsBuilder()
        .create()
        .options(row.AvailableFtzOperators)
        .build();
      row.AvailableFtzOperatorsOptions = options;
    }
  }
}
