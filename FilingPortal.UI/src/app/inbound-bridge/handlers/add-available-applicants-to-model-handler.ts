import { FieldOptions, LookupFieldOptionsBuilder } from '@common/fields/models';
import { ListOptionModel } from '@common/fields/models/ListOptionModel';

/**
 * Provides handler for extending data model with client actions property
 */
export class AddAvailableApplicantsHandler {

  static handler = (row: {
    Ein: string,
    Importer: string,
    AvailableApplicants: ListOptionModel[],
    AvailableApplicantOptions: FieldOptions
  }): void => {
    if (row.AvailableApplicants && row.AvailableApplicants.length) {
      const options = new LookupFieldOptionsBuilder()
        .create()
        .options(row.AvailableApplicants)
        .build();
      row.AvailableApplicantOptions = options;
    }
  }
}
