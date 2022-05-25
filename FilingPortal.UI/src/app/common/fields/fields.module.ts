import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { NgxMyDatePickerModule } from 'ngx-mydatepicker';
import { NgxDatatableModule } from '@custom/ngx-datatable';
import { NgxMaskModule } from 'ngx-mask';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { FieldsService } from './services/fields.service';
import { FieldsApiService } from './services/fields-api.service';

import { FormatNumberDirective } from './format-number';
import { FormatDecimalDirective } from './format-decimal';
import { DependsOnDirective } from './directives/depends-on.directive';

import { FieldSelectComponent } from './field-select';
import { FieldTextComponent } from './field-text';
import { FieldMultilineTextComponent } from './field-multiline-text';
import { FieldNumberComponent } from './field-number';
import { FieldDateComponent } from './field-date';
import { FieldFloatNumberComponent } from './field-float-number';
import { FieldBooleanComponent } from './field-boolean/field-boolean.component';
import { FieldLookupComponent } from './field-lookup/field-lookup.component';
import { FieldComplexComponent } from './field-complex/field-complex.component';
import { FieldsSelectorComponent } from './fields-selector/fields-selector.component';
import { FieldTableComponent } from './field-table/field-table.component';
import { FieldAddressComponent } from './field-address';
import { FieldConfirmationComponent } from './field-confirmation/field-confirmation.component';
import { ColumnFieldSelectorComponent } from './column-field-selector';
import { FieldCompositeComponent } from './field-composite';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    NgxMyDatePickerModule.forRoot(),
    NgxMaskModule,
    NgxDatatableModule,
    NgbModule
  ],
  exports: [
    FormsModule,
    FormatNumberDirective,
    FormatDecimalDirective,
    FieldSelectComponent,
    FieldTextComponent,
    FieldMultilineTextComponent,
    FieldNumberComponent,
    FieldDateComponent,
    FieldFloatNumberComponent,
    FieldBooleanComponent,
    FieldLookupComponent,
    FieldComplexComponent,
    FieldsSelectorComponent,
    DependsOnDirective,
    FieldTableComponent,
    FieldConfirmationComponent,
    ColumnFieldSelectorComponent,
    FieldCompositeComponent
  ],
  declarations: [
    FormatNumberDirective,
    FormatDecimalDirective,
    FieldSelectComponent,
    FieldTextComponent,
    FieldMultilineTextComponent,
    FieldNumberComponent,
    FieldDateComponent,
    FieldFloatNumberComponent,
    FieldBooleanComponent,
    FieldLookupComponent,
    FieldComplexComponent,
    FieldsSelectorComponent,
    DependsOnDirective,
    FieldTableComponent,
    FieldAddressComponent,
    FieldConfirmationComponent,
    ColumnFieldSelectorComponent,
    FieldCompositeComponent
  ],
  providers: [FieldsService, FieldsApiService]
})
export class FieldsModule { }
