import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CanActivateGuard } from '../common/guards/can-activate.guard';

import { InboundBridgePageComponent } from './inbound-bridge-page';
import { InboundBridgeRailListComponent } from './rail-list';
import { PipelineListComponent } from './pipeline-list';
import { TruckListComponent } from './truck-list';
import { VesselListComponent } from './vessel-list';
import { TruckExportListComponent } from './truck-export-list';
import { ReviewScreenComponent } from './review-screen';
import { VesselExportListComponent } from './vessel-export-list';
import { InbondListComponent } from './inbond-list/inbond-list.component';
import { CanadaImpTruckListComponent } from './canada-imp-truck-list/canada-imp-truck-list.component';
import { IsfListComponent } from './isf-list/isf-list.component';
import { UsExpRailListComponent } from './us-exp-rail-list';
import { ZonesEntryListComponent } from './zones-entry-list/zones-entry-list.component';
import { ZonesFtz214ListComponent } from './zones-ftz-214-list/zones-ftz-214-list.component';

import { InboundType } from './models';
import { Permissions, CanadaImpTruckPermissions, ZonesInBondPermissions, IsfPermissions, UsExpRailPermissions } from '@common/models';
import { ZonesEntryPermissions } from '@common/models/zones-entry-models';
import { ZonesFtz214Permissions } from '@common/models/zones-ftz214-models';

const routes: Routes = [
  {
    path: 'imports',
    component: InboundBridgePageComponent,
    data: { title: 'Imports' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'rail', pathMatch: 'full' },
      {
        path: 'rail',
        component: InboundBridgeRailListComponent,
        data: {
          title: 'Imports - Rail - US',
          type: InboundType.Rail,
          permissions: [Permissions.RailViewInboundRecord]
        }
      },
      {
        path: 'rail/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Rail,
          permissions: [Permissions.RailFileInboundRecord]
        }
      },
      {
        path: 'rail/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Rail,
          viewMode: true,
          permissions: [Permissions.RailViewInboundRecord]
        }
      },
      {
        path: 'pipeline',
        component: PipelineListComponent,
        data: {
          title: 'Imports - Pipeline - US',
          type: InboundType.Pipeline,
          permissions: [Permissions.PipelineViewInboundRecord]
        }
      },
      {
        path: 'pipeline/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Pipeline,
          hideDocuments: false,
          permissions: [Permissions.PipelineFileInboundRecord]
        }
      },
      {
        path: 'pipeline/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Pipeline,
          viewMode: true,
          hideDocuments: false,
          permissions: [Permissions.PipelineViewInboundRecord]
        }
      },
      {
        path: 'truck',
        component: TruckListComponent,
        data: {
          title: 'Imports - Truck - US',
          type: InboundType.Truck,
          permissions: [Permissions.TruckViewInboundRecord]
        }
      },
      {
        path: 'truck/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Truck,
          permissions: [Permissions.TruckFileInboundRecord]
        }
      },
      {
        path: 'truck/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Truck,
          viewMode: true,
          permissions: [Permissions.TruckViewInboundRecord]
        }
      },
      {
        path: 'vessel',
        component: VesselListComponent,
        data: {
          title: 'Imports - Vessel - US',
          type: InboundType.Vessel,
          permissions: [Permissions.VesselViewImportRecord]
        }
      },
      {
        path: 'vessel/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Vessel,
          permissions: [Permissions.VesselFileImportRecord]
        }
      },
      {
        path: 'vessel/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Vessel,
          viewMode: true,
          permissions: [Permissions.VesselViewImportRecord]
        }
      },
      {
        path: 'truck-canada',
        component: CanadaImpTruckListComponent,
        data: {
          title: 'Import - Truck - CA',
          type: InboundType.CanadaTruckImport,
          permissions: [CanadaImpTruckPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'truck-canada/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.CanadaTruckImport,
          hideDocuments: false,
          permissions: [CanadaImpTruckPermissions.FileInboundRecord]
        }
      },
      {
        path: 'truck-canada/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.CanadaTruckImport,
          viewMode: true,
          hideDocuments: false,
          permissions: [CanadaImpTruckPermissions.ViewInboundRecord]
        }
      },
    ]
  },
  {
    path: 'exports',
    component: InboundBridgePageComponent,
    data: { title: 'Exports' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'truck', pathMatch: 'full' },
      {
        path: 'rail',
        component: UsExpRailListComponent,
        data: {
          title: 'Exports - Rail - US',
          type: InboundType.RailExport,
          permissions: [UsExpRailPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'rail/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.RailExport,
          hideDocuments: false,
          permissions: [UsExpRailPermissions.FileInboundRecord]
        }
      },
      {
        path: 'rail/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.RailExport,
          viewMode: true,
          hideDocuments: false,
          permissions: [UsExpRailPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'truck',
        component: TruckExportListComponent,
        data: {
          title: 'Exports - Truck - US',
          type: InboundType.TruckExport,
          permissions: [Permissions.TruckViewExportRecord]
        }
      },
      {
        path: 'truck/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.TruckExport,
          hideDocuments: false,
          permissions: [Permissions.TruckFileExportRecord]
        }
      },
      {
        path: 'truck/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.TruckExport,
          viewMode: true,
          hideDocuments: false,
          permissions: [Permissions.TruckViewExportRecord]
        }
      },
      {
        path: 'vessel',
        component: VesselExportListComponent,
        data: {
          title: 'Exports - Vessel - US',
          type: InboundType.VesselExport,
          permissions: [Permissions.VesselViewExportRecord]
        }
      },
      {
        path: 'vessel/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.VesselExport,
          hideDocuments: false,
          permissions: [Permissions.VesselFileExportRecord]
        }
      },
      {
        path: 'vessel/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.VesselExport,
          viewMode: true,
          hideDocuments: false,
          permissions: [Permissions.VesselViewExportRecord]
        }
      }
    ]
  },
  {
    path: 'zones',
    component: InboundBridgePageComponent,
    data: { title: 'Zones' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'in-bond', pathMatch: 'full' },
      {
        path: 'in-bond',
        component: InbondListComponent,
        data: {
          title: 'Zones - In-Bond - US',
          type: InboundType.Inbond,
          permissions: [ZonesInBondPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'in-bond/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Inbond,
          hideDocuments: false,
          permissions: [ZonesInBondPermissions.FileInboundRecord]
        }
      },
      {
        path: 'in-bond/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Inbond,
          viewMode: true,
          hideDocuments: false,
          permissions: [ZonesInBondPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'entry-06',
        component: ZonesEntryListComponent,
        data: {
          title: 'Zones - Entry',
          type: InboundType.ZonesEntry,
          permissions: [ZonesEntryPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'entry-06/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.ZonesEntry,
          hideDocuments: false,
          permissions: [ZonesEntryPermissions.FileInboundRecord]
        }
      },
      {
        path: 'entry-06/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.ZonesEntry,
          viewMode: true,
          hideDocuments: false,
          permissions: [ZonesEntryPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'ftz-214',
        component: ZonesFtz214ListComponent,
        data: {
          title: 'Zones - FTZ 214',
          type: InboundType.ZonesFtz214,
          permissions: [ZonesFtz214Permissions.ViewInboundRecord]
        }
      },
      {
        path: 'ftz-214/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.ZonesFtz214,
          hideDocuments: false,
          permissions: [ZonesFtz214Permissions.FileInboundRecord]
        }
      },
      {
        path: 'ftz-214/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.ZonesFtz214,
          viewMode: true,
          hideDocuments: false,
          permissions: [ZonesFtz214Permissions.ViewInboundRecord]
        }
      },
    ]
  },
  {
    path: 'isf',
    component: InboundBridgePageComponent,
    data: { title: 'ISF' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      { path: '', redirectTo: 'inbound', pathMatch: 'full' },
      {
        path: 'inbound',
        component: IsfListComponent,
        data: {
          title: 'ISF',
          type: InboundType.Isf,
          permissions: [IsfPermissions.ViewInboundRecord]
        }
      },
      {
        path: 'inbound/review-and-file',
        component: ReviewScreenComponent,
        data: {
          title: 'Review and file',
          type: InboundType.Isf,
          hideDocuments: false,
          permissions: [IsfPermissions.FileInboundRecord]
        }
      },
      {
        path: 'inbound/view',
        component: ReviewScreenComponent,
        data: {
          title: 'View',
          type: InboundType.Isf,
          viewMode: true,
          hideDocuments: false,
          permissions: [IsfPermissions.ViewInboundRecord]
        }
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InboundBridgeRoutingModule { }
