import { FieldOptions, FieldOptionsBuilder } from './FieldOptions';
import { CompositeFieldOptions } from './composite-field-options';

export class CompositeFieldOptionsBuilder extends FieldOptionsBuilder {
  model: CompositeFieldOptions;

  create() {
    this.model = new CompositeFieldOptions();
    return this;
  }

  addOption(value: FieldOptions): CompositeFieldOptionsBuilder {
    this.model.options.push(value);
    return this;
  }
}
