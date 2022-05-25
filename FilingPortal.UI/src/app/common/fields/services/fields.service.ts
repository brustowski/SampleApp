import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { FieldRegisterItem } from '../models/field-register-item';

@Injectable()
export class FieldsService {
  private fields: Map<string, FieldRegisterItem> = new Map<string, FieldRegisterItem>();
  // TODO: current implementation may cause errors because of service lifetime. Should be replaced with more accurate implementation
  private displayValues: { [key: string]: string } = {};

  private updatedFieldSource = new Subject<string>();
  updatedField$ = this.updatedFieldSource.asObservable();

  constructor() { }

  notify(fieldName: string): void {
    this.updatedFieldSource.next(fieldName);
  }

  register(fieldName: string, valueAccessor: any, setValueAccessor: (value: any) => void): void {
    // todo: decide what to do if field already registered
    if (!this.fields.has(fieldName)) {
      this.fields.set(fieldName, new FieldRegisterItem(fieldName, valueAccessor, setValueAccessor));
    } else {
      console.warn('Field already registered.', fieldName);
    }
  }

  unregister(fieldName: string): void {
    if (this.fields.has(fieldName)) {
      this.fields.delete(fieldName);
    } else {
      console.warn(`Didn't manage to unregister.`, fieldName, this.fields.keys);
    }
  }

  getFieldValue<T>(fieldName: string): T {
    if (this.fields.has(fieldName) && this.fields.get(fieldName).valueAccessor) {
      return this.fields.get(fieldName).valueAccessor();
    }
    return null;
  }

  setDisplayValue(fieldName: string, value: string) {
    this.displayValues[fieldName] = value;
  }

  getDisplayValue(fieldName: string): string {
    return this.displayValues[fieldName];
  }

  setFieldValue(name: string, value: any) {
    const field = this.fields.get(name);
    if (field && field.setValueAccessor) {
      field.setValueAccessor(value);
      this.notify(name);
    }
  }
}
