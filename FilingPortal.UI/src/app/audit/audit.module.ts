import { NgModule } from '@angular/core';
import { CommonModule as CommonAngularModule } from '@angular/common';
import { ConsistSheetComponent } from './rail/consist-sheet/consist-sheet.component';
import { AuditPageComponent } from './audit-page/audit-page.component';
import { AuditRoutingModule } from './audit-routing.module';
import { NgxDatatableModule } from '@custom/ngx-datatable';
import { CommonModule } from '../common';
import { GridPageModule } from '@common/grid';
import { FiltersModule } from '@common/filters';
import { FileUploadModule } from 'ng2-file-upload';
import { AuditComponent } from './audit/audit.component';
import { ConfigurationService } from './services';
import { DailyAuditComponent } from './rail/daily-audit/daily-audit.component';
import { DailyAuditRulesComponent } from './rail/daily-audit-rules/daily-audit-rules.component';
import { RouterModule } from '@angular/router';
import { DailyAuditSpiRulesComponent } from './rail/daily-audit-spi-rules/daily-audit-spi-rules.component';

@NgModule({
  declarations: [
    ConsistSheetComponent,
    AuditPageComponent,
    AuditComponent,
    DailyAuditComponent,
    DailyAuditRulesComponent,
    DailyAuditSpiRulesComponent],
  imports: [
    CommonAngularModule,
    RouterModule,
    AuditRoutingModule,
    CommonModule,
    NgxDatatableModule,
    GridPageModule,
    FiltersModule,
    FileUploadModule
  ],
  providers: [
    { provide: 'GridsConfigurationService', useClass: ConfigurationService }
  ]
})
export class AuditModule { }
