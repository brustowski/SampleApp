import { Field } from './field';
import { FieldOptions } from './FieldOptions';

export class FieldBuilder {
    model: Field;

    create(): FieldBuilder {
        this.model = new Field();
        return this;
    }

    name(value: string): FieldBuilder {
        this.model.name = value;
        return this;
    }

    title(value: string): FieldBuilder {
        this.model.title = value;
        return this;
    }

    value(value: string): FieldBuilder {
        this.model.value = value;
        return this;
    }

    options(options: FieldOptions): FieldBuilder {
        this.model.options = options;
        return this;
    }

    type(value: string): FieldBuilder {
        this.model.type = value;
        return this;
    }

    build(): Field {
        return this.model;
    }
}
