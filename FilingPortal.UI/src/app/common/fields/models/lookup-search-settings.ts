import { SourceType } from './source-type';

export class LookupSearchSettings {
    public sourceName: string;
    public sourceType: SourceType;
    public fieldName: string;
    public dependValue: string;
    public dependOn: string;
    public searchText: string;
    public limit: number = 0;
    public searchByKey: boolean = false;
}
