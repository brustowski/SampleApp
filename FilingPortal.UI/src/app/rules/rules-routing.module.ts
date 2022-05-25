import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RulesPageComponent } from './rules-page/rules-page.component';
import { RuleListComponent } from './rule-list/rule-list.component';
import { CanDeactivateGuard } from '../common/guards/can-deactivate.guard';
import { RulesComponent } from './rules/rules.component';
import { CanActivateGuard } from '../common/guards/can-activate.guard';
import { Permissions } from '@common/models';
import { CanadaImpTruckPermissions, ZonesInBondPermissions, IsfPermissions, UsExpRailPermissions } from '@common/models';
import { ZonesEntryPermissions } from '@common/models/zones-entry-models';
import { ZonesFtz214Permissions } from '@common/models/zones-ftz214-models';

const routes: Routes = [
  {
    path: 'rules',
    component: RulesPageComponent,
    data: { title: 'Rules' },
    canActivate: [CanActivateGuard],
    canActivateChild: [CanActivateGuard],
    children: [
      {
        path: 'rail',
        component: RulesComponent,
        data: { title: 'Rules - Rail - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: { title: 'Rules - Rail - US', permissions: [Permissions.RailViewInboundRecordRules, UsExpRailPermissions.ViewRules] },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'pipeline',
        component: RulesComponent,
        data: { title: 'Rules - Pipeline - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: { title: 'Rules - Pipeline - US', permissions: [Permissions.PipelineViewInboundRecordRules] },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'truck',
        component: RulesComponent,
        data: { title: 'Rules - Truck - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - Truck - US',
              permissions: [Permissions.TruckViewInboundRecordRules, Permissions.TruckViewExportRecordRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'vessel',
        component: RulesComponent,
        data: { title: 'Rules - Vessel - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - Vessel - US',
              permissions: [Permissions.VesselViewImportRecordRules, Permissions.VesselViewExportRecordRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'zones-inbond',
        component: RulesComponent,
        data: { title: 'Rules - In-Bond - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - In-Bond - US',
              permissions: [ZonesInBondPermissions.ViewRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'zones-entry-06',
        component: RulesComponent,
        data: { title: 'Rules - Entry 06 - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - Entry 06 - US',
              permissions: [ZonesEntryPermissions.ViewRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'zones-ftz-214',
        component: RulesComponent,
        data: { title: 'Rules - FTZ 214 - US' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - FTZ 214 - US',
              permissions: [ZonesFtz214Permissions.ViewRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
      {
        path: 'canada-imp-truck',
        component: RulesComponent,
        data: { title: 'Rules - Truck - CA' },
        children: [
          {
            path: ':name',
            component: RuleListComponent,
            data: {
              title: 'Rules - Truck - CA',
              permissions: [CanadaImpTruckPermissions.ViewRules]
            },
            canDeactivate: [CanDeactivateGuard]
          }
        ]
      },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class RulesRoutingModule { }
