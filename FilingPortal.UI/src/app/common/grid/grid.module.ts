import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@custom/ngx-datatable';

import { GridPageApiService } from './services/grid-page-api.service';
import { GridPageColumnsService } from './services/grid-page-columns.service';
import { GridService } from './services/grid.service';
import { GridStorageService } from './services/grid-storage.service';

import { ColumnFilterPipe } from './pipes';

import { GridToolbarComponent } from './grid-toolbar';
import { GridCountResultsComponent } from './grid-count-results';
import { GridSelectionToolbarComponent } from './grid-selection-toolbar';
import { GridHeaderComponent } from './grid-header/grid-header.component';
import { ColumnManagementButtonComponent } from './column-management-button/column-management-button.component';
import { ColumnManagementComponent } from './column-management/column-management.component';

@NgModule({
    declarations: [
      GridToolbarComponent,
      GridCountResultsComponent,
      GridSelectionToolbarComponent,
      GridHeaderComponent,
      ColumnFilterPipe,
      ColumnManagementButtonComponent,
      ColumnManagementComponent
    ],
    providers: [
      GridPageApiService,
      GridPageColumnsService,
      GridService,
      GridStorageService,
      ColumnFilterPipe
    ],
    imports: [
      HttpClientModule,
      CommonModule,
      FormsModule,
      NgbModule,
      NgxDatatableModule
    ],
    exports: [
        GridToolbarComponent,
        GridCountResultsComponent,
        GridSelectionToolbarComponent,
        GridHeaderComponent,
        ColumnManagementButtonComponent
    ],
    entryComponents: [
      ColumnManagementComponent
    ]
})
export class GridPageModule { }
