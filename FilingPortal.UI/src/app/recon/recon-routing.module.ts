import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateGuard } from '@common/guards/can-activate.guard';
import { InboundType } from '@inbound/models';
import { ReconComponent } from './recon';
import { ReconPageComponent } from './recon-page';
import { ReconPermissions } from '@common/models';
import { CargoWiseListComponent } from './cargowise-list';
import { FtaReconListComponent } from './fta-recon-list';
import { ValueReconListComponent } from './value-recon-list';

const routes: Routes = [
  {
    path: 'recon',
    component: ReconPageComponent,
    data: { title: 'Recon' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'main', pathMatch: 'full' },
      {
        path: 'main',
        component: ReconComponent,
        data: { title: 'Report' },
        children: [
          { path: '', redirectTo: 'cargowise-report', pathMatch: 'full' },
          {
            path: 'cargowise-report',
            component: CargoWiseListComponent,
            data: {
              title: 'CargoWise Report',
              name: 'cargowise-report',
              type: InboundType.Recon,
              permissions: [ReconPermissions.ViewInboundRecord]
            }
          },
          {
            path: 'fta',
            component: FtaReconListComponent,
            data: {
              title: 'FTA Recon',
              name: 'fta',
              type: InboundType.FtaRecon,
              permissions: [ReconPermissions.ViewFtaRecord]
            }
          },
          {
            path: 'value',
            component: ValueReconListComponent,
            data: {
              title: 'Value Recon',
              name: 'value',
              type: InboundType.ValueRecon,
              permissions: [ReconPermissions.ViewValueRecord]
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
export class ReconRoutingModule { }
