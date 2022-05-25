export class ColumnServer {
    Align: number = 1; // 1, 2 - 'a-center', 3 - 'a-right'
    DefaultSorted: boolean = false;
    DisplayName: string = '';
    FieldName: string = '';
    KeyFieldName: string;
    IsSortable: boolean = false;
    IsViewOpen: boolean = false;
    IsResizable: boolean = false;
    IsSearchable: boolean = false;
    IsKeyField: boolean = false;
    MaxWidth: number = 200;
    MinWidth: number = 50;
    EditType: string = 'text';
    DependOn: string = '';
    DependOnProperty: string = '';
    Columns: ColumnServer[];
}
