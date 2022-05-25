import { Directive, OnDestroy, OnInit } from '@angular/core';

import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import * as R from 'ramda';

import { FieldsService } from '../services/fields.service';

import { RequiredField } from '../field-required-ctrl';
import { isUndefined } from 'util';

@Directive({
  selector: `lxft-field-text [lxftDependsOn]
  , lxft-field-multiline-text [lxftDependsOn]
  , lxft-field-float-number [lxftDependsOn]
  , lxft-field-date [lxftDependsOn]
  , lxft-field-lookup [lxftDependsOn]`
})
export class DependsOnDirective implements OnInit, OnDestroy {
  private valueChangeSub: Subscription;
  private fieldChangedSub: Subscription;
  private fieldName: string;
  private dependsOnValueChangeSub: Subscription;
  private dependsOn: string;
  private dependsOnValues: string[];

  constructor(private fieldsService: FieldsService, private component: RequiredField) { }

  ngOnInit(): void {
    this.fieldChangedSub = this.component.onFieldChanged.subscribe(() => this.proccesDependency());
    this.valueChangeSub = this.component.valueChange
      .debounceTime(600)
      .distinctUntilChanged()
      .filter(() => !!this.fieldName)
      .subscribe(() => this.fieldsService.notify(this.fieldName));
    this.dependsOnValueChangeSub = this.fieldsService.updatedField$
      .pipe(filter(fieldName => fieldName && this.dependsOn && this.dependsOn.toLowerCase() === fieldName.toLowerCase()))
      .subscribe(() => {
        this.component.clear(true);
        this.changeDisabledStatus();
      });
    this.proccesDependency();
  }

  private proccesDependency() {
    const options = this.component.options;
    if (this.fieldName && this.fieldName !== options.name) {
      this.fieldsService.unregister(this.fieldName);
    }
    if (options.name) {
      this.fieldName = options.name;
      this.fieldsService.register(this.fieldName, () => this.component.value, x => this.component.value = x);
      if (options.dependsOn) {
        this.dependsOn = options.dependsOn;
        this.dependsOnValues = options.dependsOnValues;
        this.changeDisabledStatus();
      }
    }
  }

  private changeDisabledStatus() {
    const val = this.fieldsService.getFieldValue(this.dependsOn);
    let isDisabled = false;
    if (isUndefined(val) || R.isEmpty(val)) {
      isDisabled = true;
    } else {
      if (
        R.not(R.isNil(this.dependsOnValues) || R.isEmpty(this.dependsOnValues) || R.contains(val, this.dependsOnValues))
      ) {
        isDisabled = true;
      }
    }
    this.component.options.isDisabled = isDisabled;
  }

  ngOnDestroy(): void {
    this.fieldsService.unregister(this.component.options.name);
    if (this.dependsOnValueChangeSub) {
      this.dependsOnValueChangeSub.unsubscribe();
    }
    if (this.valueChangeSub) {
      this.valueChangeSub.unsubscribe();
    }
    if (this.fieldChangedSub) {
      this.fieldChangedSub.unsubscribe();
    }
  }
}
