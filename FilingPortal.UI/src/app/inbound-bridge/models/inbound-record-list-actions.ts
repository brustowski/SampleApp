import { AvailableActions } from '@common/models';

export class InboundRecordListActions extends AvailableActions {
    ReviewFile: boolean = false;
    Undo: boolean = false;
    Edit: boolean = false;
    View: boolean = false;
    SelectAll: boolean = false;
    SingleFiling: boolean = false;
    Delete: boolean = false;
    Restore: boolean = false;
    DocumentsUpload: boolean = false;
    Apprrove: boolean = false;
}
