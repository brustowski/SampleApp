import { NgModule } from '@angular/core';
import { CommonModule as CommonAngularModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NgxDatatableModule } from '@custom/ngx-datatable';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FileUploadModule } from 'ng2-file-upload';

import { CommonModule } from '../common';
import { GridPageModule } from '@common/grid';
import { FiltersModule } from '@common/filters';
import { ConfigurationService } from './services';
import { ReconRoutingModule } from './recon-routing.module';
import { ReconComponent } from './recon';
import { ReconPageComponent } from './recon-page';
import { CargoWiseListComponent } from './cargowise-list';
import { FtaReconListComponent } from './fta-recon-list';
import { ValueReconListComponent } from './value-recon-list';
import { CargowiseListExportComponent } from './cargowise-list-export';
import { FtaReconExportButtonComponent } from './fta-recon-export-button';
import { ValueReconExportButtonComponent } from './value-recon-export-button/value-recon-export-button.component';
import { ReconFiltersPanelComponent } from './recon-filters-panel';

@NgModule({
  declarations: [
    CargoWiseListComponent,
    FtaReconListComponent,
    ValueReconListComponent,
    ReconPageComponent,
    ReconComponent,
    CargowiseListExportComponent,
    FtaReconExportButtonComponent,
    ValueReconExportButtonComponent,
    ReconFiltersPanelComponent
  ],
  imports: [
    CommonAngularModule,
    RouterModule,
    FormsModule,
    ReconRoutingModule,
    CommonModule,
    NgxDatatableModule,
    GridPageModule,
    FiltersModule,
    FileUploadModule,
    NgbModule,
  ],
  providers: [
    { provide: 'ReconGridsConfigurationService', useClass: ConfigurationService }
  ]
})
export class ReconModule { }
