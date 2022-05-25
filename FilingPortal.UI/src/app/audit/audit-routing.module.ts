import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Permissions } from '@common/models';

import { CanActivateGuard } from '@common/guards/can-activate.guard';
import { InboundType } from '@inbound/models';
import { ConsistSheetComponent } from './rail/consist-sheet';
import { AuditPageComponent } from './audit-page';
import { CanDeactivateGuard } from '@common/guards/can-deactivate.guard';
import { AuditComponent } from './audit';
import { DailyAuditComponent } from './rail/daily-audit/daily-audit.component';
import { DailyAuditRulesComponent } from './rail/daily-audit-rules/daily-audit-rules.component';
import { DailyAuditSpiRulesComponent } from './rail/daily-audit-spi-rules/daily-audit-spi-rules.component';

const routes: Routes = [
  {
    path: 'audit',
    component: AuditPageComponent,
    data: { title: 'Audit' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'rail', pathMatch: 'full' },
      {
        path: 'rail',
        component: AuditComponent,
        data: { title: 'Audit - Rail - US' },
        children: [
          { path: '', redirectTo: 'consist-sheet', pathMatch: 'full' },
          {
            path: 'consist-sheet',
            component: ConsistSheetComponent,
            data: {
              title: 'Consist Sheet',
              name: 'consist-sheet',
              type: InboundType.Rail,
              permissions: [Permissions.AuditRailImportTrainConsistSheet]
            }
          },
          {
            path: 'daily-audit',
            component: DailyAuditComponent,
            data: {
              title: 'Daily audit',
              name: 'daily-audit',
              type: InboundType.DailyAudit,
              permissions: [Permissions.AuditRailDailyAudit]
            }
          },
          {
            path: 'daily-audit-rules',
            component: DailyAuditRulesComponent,
            data: {
              title: 'Daily audit rules',
              name: 'daily-audit-rules',
              type: InboundType.DailyAudit,
              permissions: [Permissions.AuditRailDailyAudit]
            }
          },
          {
            path: 'daily-audit-spi-rules',
            component: DailyAuditSpiRulesComponent,
            data: {
              title: 'Daily audit SPI rules',
              name: 'daily-audit-spi-rules',
              type: InboundType.DailyAudit,
              permissions: [Permissions.AuditRailDailyAudit]
            }
          },
        ]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditRoutingModule {}
