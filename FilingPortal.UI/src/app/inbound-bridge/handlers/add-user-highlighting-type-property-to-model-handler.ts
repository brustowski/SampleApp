import { HighlightingType } from '@common/models';

/**
 * Provides handler for extending data model with client actions property
 */
export class AddUserHighlightingTypePropertyToModelHandler {

  static handler = (row: any): void => {
    row.UserHighlightingType = HighlightingType.NoHighlighting;
  }
}
