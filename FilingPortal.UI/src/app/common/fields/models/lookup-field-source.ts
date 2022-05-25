import { SourceType } from './source-type';

export class LookupFieldSource {
  public isDynamic: boolean = false;

  constructor(public name: string, public type: SourceType, public canAdd: boolean, public propertyName?: string) { }
}
