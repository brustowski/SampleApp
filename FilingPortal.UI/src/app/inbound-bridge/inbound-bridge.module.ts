import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule as CommonAngularModule } from '@angular/common';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { FileUploadModule } from 'ng2-file-upload';

import { InboundBridgeRoutingModule } from './inbound-bridge-routing.module';

import { CommonModule } from '../common';

import { NgxDatatableModule } from '@custom/ngx-datatable';
import { GridPageModule } from '../common/grid';
import { FiltersModule } from '../common/filters';
import { InboundBridgePageComponent } from './inbound-bridge-page/inbound-bridge-page.component';

import {
  InboundRecordsApiService,
  InboundRecordsService,
  InboundRecordsValidator,
  InboundConfigurationService,
  SingleFilingService,
  FilingParametersService,
  AddInboundRecordService
} from './services';
import { DocumentsSectionComponent } from './documents-section';
import { DocumentsTableSectionComponent } from './documents-table-section';
import { TruckListComponent } from './truck-list';
import { VesselListComponent } from './vessel-list';
import { ManifestComponent } from './manifest';
import { InboundBridgeRailListComponent } from './rail-list';
import { PipelineListComponent } from './pipeline-list';
import { AddVesselModalComponent } from './add-vessel-modal/add-vessel-modal.component';
import { EditVesselModalComponent } from './edit-vessel-modal/edit-vessel-modal.component';
import { TruckExportListComponent } from './truck-export-list';
import { VesselExportListComponent } from './vessel-export-list';
import { ReviewScreenComponent } from './review-screen';

import { ExpandableGridFilingComponent } from './expandable-grid-filing';
import { ExpandableGridDetailedRowComponent } from './expandable-grid-detailed-row';
import { FilingParametersTreeComponent } from './filing-parameters-tree';
import { FilingParametersTreeNodeComponent } from './filing-parameters-tree-node';
import { FilingParametersTreeNodeCollapsibleComponent } from './filing-parameters-tree-node-collapsible';
import { FilingParametersTreeDocumentTabComponent } from './filing-parameters-tree-document-tab';
import { FilingParametersTreeNodeFieldsComponent } from './filing-parameters-tree-node-fields';
import { RailManifestDataComponent } from './rail-manifest-data/rail-manifest-data.component';
import { PipelineMassUploadComponent } from './pipeline-mass-upload/pipeline-mass-upload.component';
import { AddManifestModalComponent } from './add-manifest-modal/add-manifest-modal.component';
import { InbondListComponent } from './inbond-list/inbond-list.component';
import { CanadaImpTruckListComponent } from './canada-imp-truck-list/canada-imp-truck-list.component';
import { IsfListComponent } from './isf-list/isf-list.component';
import { MarksRemarksComponent } from './marks-remarks/marks-remarks.component';
import { MarksRemarksService } from './services/marks-remarks.service';
import { IsfAddInboundModalComponent } from './isf-add-inbound-modal';
import { UsExpRailListComponent } from './us-exp-rail-list';
import { GridDataComponent } from './grid-data/grid-data.component';
import { ZonesEntryListComponent } from './zones-entry-list/zones-entry-list.component';
import { FilingParametersTreeGridComponent } from './filing-parameters-tree-grid/filing-parameters-tree-grid.component';
import { TruckExportFilingConfirmationDialogComponent } from './truck-export-filing-confirmation-dialog';
import { FilingParametersTreeGridImportComponent } from './filing-parameters-tree-grid-import/filing-parameters-tree-grid-import.component';
import { ZonesFtz214ListComponent } from './zones-ftz-214-list/zones-ftz-214-list.component';


@NgModule({
  imports: [
    InboundBridgeRoutingModule,
    CommonAngularModule,
    FormsModule,
    CommonModule,
    FiltersModule,
    NgxDatatableModule,
    GridPageModule,
    NgbModule,
    NgSelectModule,
    FileUploadModule,
  ],
  declarations: [
    InboundBridgePageComponent,
    DocumentsSectionComponent,
    DocumentsTableSectionComponent,
    TruckListComponent,
    VesselListComponent,
    ManifestComponent,
    InboundBridgeRailListComponent,
    PipelineListComponent,
    AddVesselModalComponent,
    EditVesselModalComponent,
    FilingParametersTreeComponent,
    TruckExportListComponent,
    VesselExportListComponent,
    ReviewScreenComponent,
    ExpandableGridFilingComponent,
    FilingParametersTreeNodeComponent,
    FilingParametersTreeDocumentTabComponent,
    ExpandableGridDetailedRowComponent,
    FilingParametersTreeNodeCollapsibleComponent,
    FilingParametersTreeNodeFieldsComponent,
    RailManifestDataComponent,
    PipelineMassUploadComponent,
    AddManifestModalComponent,
    IsfAddInboundModalComponent,
    InbondListComponent,
    CanadaImpTruckListComponent,
    MarksRemarksComponent,
    IsfListComponent,
    UsExpRailListComponent,
    GridDataComponent,
    ZonesEntryListComponent,
    ZonesFtz214ListComponent,
    FilingParametersTreeGridComponent,
    TruckExportFilingConfirmationDialogComponent,
    FilingParametersTreeGridImportComponent,
  ],
  providers: [
    InboundConfigurationService,
    InboundRecordsApiService,
    InboundRecordsService,
    SingleFilingService,
    AddInboundRecordService,
    FilingParametersService,
    InboundRecordsValidator,
    MarksRemarksService
  ],
  entryComponents: [
    ManifestComponent,
    AddVesselModalComponent,
    EditVesselModalComponent,
    PipelineMassUploadComponent,
    AddManifestModalComponent,
    FilingParametersTreeDocumentTabComponent,
    IsfAddInboundModalComponent,
    TruckExportFilingConfirmationDialogComponent
  ]
})
export class InboundBridgeModule { }
