/**
 * Provides handler for extending data model with ViewMode property
 */
export class AddViewModeToModelHandler {
    static handler = (row: any): void => {
        row.ViewMode = true;
    }
}
