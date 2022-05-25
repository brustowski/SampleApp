import { Injectable, OnDestroy } from '@angular/core';
import { FieldsApiService } from '@common/fields/services/fields-api.service';
import { Observable, of, Subscription } from 'rxjs';
import { MarksRemarksTemplate, ZonesInBondDataProviderNames } from '@common/models/zones-inbond-models';
import { LookupSearchSettings, SourceType } from '@common/fields/models';
import { FilingParametersService, InboundConfigurationService } from '.';
import { InboundRecordParameter, InboundType } from '@inbound/models';

@Injectable({
  providedIn: 'root'
})
export class MarksRemarksService implements OnDestroy {
  private _templates: MarksRemarksTemplate[];
  private _entryType: string;

  private _templateType: InboundRecordParameter;
  get templateType(): string {
    return this._templateType && this._templateType.value ? this._templateType.value : null;
  }
  set templateType(value: string) {
    if (this._templateType) {
      this._templateType.value = value;
    }
  }

  private _descriptionTemplate: InboundRecordParameter;
  get descriptionTemplate(): string {
    return this._descriptionTemplate ? this._descriptionTemplate.value : null;
  }
  set descriptionTemplate(value: string) {
    if (this._descriptionTemplate) {
      this._descriptionTemplate.value = value;
    }
  }

  private _marksNumbersTemplate: InboundRecordParameter;
  get marksNumbersTemplate(): string {
    return this._marksNumbersTemplate ? this._marksNumbersTemplate.value : null;
  }
  set marksNumbersTemplate(value: string) {
    if (this._marksNumbersTemplate) {
      this._marksNumbersTemplate.value = value;
    }
  }

  private subscription: Subscription;

  constructor(private fieldsService: FieldsApiService,
    private filingParametersService: FilingParametersService,
    private configurationService: InboundConfigurationService) {
    this.subscription = this.filingParametersService.filingConfiguration$.subscribe(() => {
      const dt = this.configurationService.getInboundType();
      if (dt === InboundType.Inbond) {
        const entry_type = this.filingParametersService.getFieldByName('in_bond_entry_type', 'movement_header');
        this._entryType = entry_type ? entry_type.value : undefined;
        this._templateType = this.filingParametersService.getFieldByName('template_type', 'commodities');
        this._descriptionTemplate = this.filingParametersService.getFieldByName('description', 'commodities');
        this._marksNumbersTemplate = this.filingParametersService.getFieldByName('marks_and_numbers', 'commodities');
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  getTemplates(): Observable<MarksRemarksTemplate[]> {
    if (this._templates && this._templates.length) {
      return of(this._templates.filter(t => t.EntryType === this._entryType));
    }
    const searchSettings = new LookupSearchSettings();
    searchSettings.sourceType = SourceType.Form;
    searchSettings.sourceName = ZonesInBondDataProviderNames.MarksRemarksTemplateTypes;
    return this.fieldsService.getLookupFieldValue(searchSettings)
      .do(r => this._templates = r.map(val => val.Value))
      .switchMap(() => of(this._templates.filter(t => t.EntryType === this._entryType)));
  }

  getTemplate(templateType: string): MarksRemarksTemplate {
    return this._templates.filter(t => t.EntryType === this._entryType && t.TemplateType === templateType)[0];
  }
}
