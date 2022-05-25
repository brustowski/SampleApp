export class FilterSearchSettings {
    gridName: string = '';
    fieldName: string = '';
    search: string = '';
    limit: number = 20;
    dependOn: string = '';
    dependValue: string = '';
}

export class FilterSearchSettingsBuilder {
    model: FilterSearchSettings;

    constructor() {}

    create() {
        this.model = new FilterSearchSettings();
        return this;
    }

    gridName(gridName: string = '') {
        this.model.gridName = gridName;
        return this;
    }

    fieldName(fieldName: string = '') {
        this.model.fieldName = fieldName;
        return this;
    }

    searchString(search: string = '') {
        this.model.search = search;
        return this;
    }

    limit(limit: number = 20) {
        this.model.limit = limit;
        return this;
    }

    dependOn(dependOn: string = '') {
        this.model.dependOn = dependOn;
        return this;
    }

    dependValue(dependValue: string = '') {
        this.model.dependValue = dependValue;
        return this;
    }

    build() {
        return this.model;
    }
}
