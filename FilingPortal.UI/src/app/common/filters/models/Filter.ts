import { SelectFilterItem } from './select-filter-item';

export class Filter {
    title: string = '';
    fieldName: string = '';
    gridName: string = '';
    type: string = '';
    operand: string = '';
    options: Array<SelectFilterItem> = [];
    dependOn: string = '';
    search: boolean = false;
    isShowTopList: boolean = false;
    maxLength: number = 0;
    value: any = null;
    defaultValue: any = null;
    disableBackendSearch: boolean = false;
    control: any = {};
    onChange = [];
    advanced: boolean = false;
    isUpdateFilter: boolean = false;
    isMultiSelect: boolean = false;

    callOnChange() {
        this.onChange.forEach(onChangeFun => {
            onChangeFun();
        });
    }
}

export interface FilterParsed {
    FieldName: string;
    Operand: string;
    Values: any[];
}

export interface FilterParsedConfig {
    Filters: FilterParsed[];
    ExcludedRecordsId?: number[];
}

export class FilterServer {
    Title: string;
    FieldName: string;
    Type: string;
    Operand: string;
    DependOn: string;
    IsSearch: boolean;
    MaxLength: number;
    Advanced: boolean;
    IsUpdateFilter: boolean;
    IsMultiSelect: boolean;
}

export class FilterBuilder {
    model: Filter;
    constructor() { }

    create() {
        this.model = new Filter();
        return this;
    }

    title(title = '') {
        this.model.title = title;
        return this;
    }

    fieldName(fieldName = '') {
        this.model.fieldName = fieldName;
        return this;
    }

    gridName(gridName = '') {
        this.model.gridName = gridName;
        return this;
    }

    type(type = '') {
        this.model.type = type;
        return this;
    }

    operand(operand = '') {
        this.model.operand = operand;
        return this;
    }

    options(options = []) {
        this.model.options = options;
        return this;
    }

    dependOn(dependOn = '') {
        this.model.dependOn = dependOn;
        return this;
    }

    isSearch(isSearch = false) {
        this.model.search = isSearch;
        return this;
    }

    showTopList(isShowTopList = false) {
        this.model.isShowTopList = isShowTopList;
        return this;
    }

    maxLength(length = 0) {
        this.model.maxLength = length > 0 ? length : null;
        return this;
    }

    advanced(isAdvanced = false) {
        this.model.advanced = isAdvanced;
        return this;
    }

    isUpdateFilter(isUpdateFilter: boolean) {
        this.model.isUpdateFilter = isUpdateFilter;
        return this;
    }

    isMultiSelect(value: boolean) {
        this.model.isMultiSelect = value;
        return this;
    }

    build() {
        return this.model;
    }
}
