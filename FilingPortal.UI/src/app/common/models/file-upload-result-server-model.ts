export interface FileUploadResultServerModel {
    ValidationErrors: any[];
    ParsingErrors: any[];
    CommonErrors: string[];
    FileName: string;
    Count: number;
}
