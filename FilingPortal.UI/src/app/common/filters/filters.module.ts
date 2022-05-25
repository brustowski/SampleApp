import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';

import { FieldsModule } from '../fields';

import { FiltersPanelComponent } from './filters-panel';
import { FilterSelectComponent } from './filter-select';
import { FilterNumberComponent } from './filter-number';
import { FilterDateRangeComponent } from './filter-date-range';

import { FiltersApiService } from './services/filters-api.service';
import { FilterService } from './services/filter.service';
import { FiltersStorageService } from './services/filters-storage.service';
import { FilterFloatNumberComponent } from './filter-float-number/filter-float-number.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BasicFiltersPipe } from './pipes/BasicFiltersPipe';
import { AdvancedFilterPipe } from './pipes/AdvancedFiltersPipe';
import { MyDateRangePickerModule } from 'mydaterangepicker';
import { ReportFiltersPanelComponent } from './report-filters-panel';

@NgModule({
    imports: [
        CommonModule,
        NgSelectModule,
        FormsModule,
        FieldsModule,
        NgbModule,
        MyDateRangePickerModule
    ],
    declarations: [
        FiltersPanelComponent,
        FilterSelectComponent,
        FilterNumberComponent,
        FilterFloatNumberComponent,
        BasicFiltersPipe,
        AdvancedFilterPipe,
        FilterDateRangeComponent,
        ReportFiltersPanelComponent
    ],
    exports: [
        FiltersPanelComponent,
        FilterSelectComponent,
        FilterNumberComponent,
        FilterFloatNumberComponent,
        BasicFiltersPipe,
        AdvancedFilterPipe,
        FilterDateRangeComponent,
        ReportFiltersPanelComponent
    ],
    providers: [
        FiltersApiService,
        FilterService,
        FiltersStorageService
    ]
})
export class FiltersModule { }
