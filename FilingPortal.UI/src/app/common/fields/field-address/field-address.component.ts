import { Component, OnInit, Input } from '@angular/core';
import { RequiredField } from '../field-required-ctrl';
import { AddressFieldEditModel, DataProviderNames } from '@common/models';
import { FieldOptions, LookupFieldOptionsBuilder, LookupFieldSource, SourceType } from '../models';
import { isObject, isNullOrUndefined } from 'util';

@Component({
  selector: 'lxft-field-address',
  templateUrl: './field-address.component.html'
})
export class FieldAddressComponent extends RequiredField implements OnInit {

  id: number;
  addressId: string;
  addressCode: string;
  override: boolean = false;
  companyName: string;
  address1: string;
  address2: string;
  countryCode: string;
  city: string;
  postalCode: string;
  stateCode: string;

  @Input() set value(val: any) {
    if (isNullOrUndefined(val)) {
      return;
    }
    let value: AddressFieldEditModel;
    if (isObject(val)) {
      value = val;
    } else {
      try {
        value = JSON.parse(val);
      } catch (error) {
        return;
      }
    }
    this.id = value.Id;
    this.addressId = value.AddressId;
    this.addressCode = value.AddressCode;
    this.override = value.Override;
    this.address1 = value.Addr1;
    this.address2 = value.Addr2;
    this.countryCode = value.CountryCode;
    this.city = value.City;
    this.postalCode = value.PostalCode;
    this.stateCode = value.StateCode;
    this.companyName = value.CompanyName;
  }
  get value(): any {
    return <AddressFieldEditModel>{
      Id: this.id,
      AddressId: this.addressId,
      Override: this.override,
      CompanyName: this.companyName,
      Addr1: this.address1,
      Addr2: this.address2,
      CountryCode: this.countryCode,
      City: this.city,
      PostalCode: this.postalCode,
      StateCode: this.stateCode,
    };
  }

  get viewModeValue(): string {
    return this.override ? this.address1 : this.addressCode;
  }

  constructor() { super(); }

  countryOptions: FieldOptions;

  ngOnInit() {
    if (this.viewMode) {
      return;
    }

    this.countryOptions = new LookupFieldOptionsBuilder()
      .create()
      .source(new LookupFieldSource(DataProviderNames.CountryCodes, SourceType.Form, false))
      .build();

    this.checkRequired(this.value);
  }

  onValueChange() {
    this.valueChange.emit(this.value);
  }
}
