import { UserActions } from '@common/grid/models';

/**
 * Provides handler for extending data model with client actions property
 */
export class AddUserActionsPropertyToModelHandler {

  static handler = (row: any): void => {
    row.UserActions = new UserActions();
  }
}
