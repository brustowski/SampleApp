export enum PageMode{
    new, waiting, inactive, requestSent
}

export interface SendRequestModel{
    mode: PageMode,
    requestInfo?: string;
}