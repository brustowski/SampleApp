import { ListOptionModel } from './ListOptionModel';

export class SelectModel {
    public isSearch = true;
    public isUseBackendOptions = true;
    public limit = 20;
    public options: Array<ListOptionModel> = [];
    public placeholder: string;
    public providerName: string;
    constructor() {}
}

export class SelectModelBuilder {
    private model: SelectModel;

    constructor() {}

    create() {
        this.model = new SelectModel();
        return this;
    }

    limitOptions(limit = 20) {
        this.model.limit = limit;
        return this;
    }

    search(isSearch = true) {
        this.model.isSearch = isSearch;
        return this;
    }

    backendOptions(isUseBackendOptions = true) {
        this.model.isUseBackendOptions = isUseBackendOptions;
        return this;
    }

    placeholder(placeholder: string) {
        this.model.placeholder = placeholder;
        return this;
    }

    providerName(providerName: string) {
        this.model.providerName = providerName;
        return this;
    }

    options(options: Array<ListOptionModel>) {
        this.model.options = options;
        return this;
    }

    build() {
        return this.model;
    }
}
