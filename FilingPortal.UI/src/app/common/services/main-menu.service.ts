import { Injectable } from '@angular/core';
import {
  MenuItem,
  Permissions,
  UsExpRailPermissions,
  ReconPermissions,
  CanadaImpTruckPermissions,
  ZonesInBondPermissions,
  IsfPermissions,
  ZonesEntryPermissions
} from '@common/models';
import { ZonesFtz214Permissions } from '@common/models/zones-ftz214-models';

@Injectable()
export class MainMenuService {
  constructor() { }

  getMenuItems(): MenuItem[] {
    return [
      {
        title: 'Dashboard',
        url: '/',
        iconClass: 'icon-dashboard',
        notSelectOnChildActivation: true
      },
      {
        title: 'Imports',
        url: '/imports',
        children: [
          {
            title: 'Rail - US',
            url: '/imports/rail',
            permissions: [Permissions.RailViewInboundRecord],
            iconClass: 'icon-rail'
          },
          {
            title: 'Pipeline - US',
            url: '/imports/pipeline',
            permissions: [Permissions.PipelineViewInboundRecord],
            iconClass: 'icon-pipeline'
          },
          {
            title: 'Truck - US',
            url: '/imports/truck',
            permissions: [Permissions.TruckViewInboundRecord],
            iconClass: 'icon-truck'
          },
          {
            title: 'Vessel - US',
            url: '/imports/vessel',
            permissions: [Permissions.VesselViewImportRecord],
            iconClass: 'icon-vessel'
          },
          {
            title: 'Truck - CA',
            url: '/imports/truck-canada',
            permissions: [
              CanadaImpTruckPermissions.ViewInboundRecord
            ],
            iconClass: 'icon-truck'
          },
        ],
        permissions: [
          Permissions.RailViewInboundRecord,
          Permissions.PipelineViewInboundRecord,
          Permissions.TruckViewInboundRecord,
          Permissions.VesselViewImportRecord,
          CanadaImpTruckPermissions.ViewInboundRecord
        ],
        iconClass: 'icon-cloud-in'
      },
      {
        title: 'Exports',
        url: '/exports',
        children: [
          {
            title: 'Rail - US',
            url: '/exports/rail',
            permissions: [UsExpRailPermissions.ViewInboundRecord],
            iconClass: 'icon-rail'
          },
          {
            title: 'Truck - US',
            url: '/exports/truck',
            permissions: [Permissions.TruckViewExportRecord],
            iconClass: 'icon-truck'
          },
          {
            title: 'Vessel - US',
            url: '/exports/vessel',
            permissions: [Permissions.VesselViewExportRecord],
            iconClass: 'icon-vessel'
          }
        ],
        permissions: [
          Permissions.TruckViewExportRecord, Permissions.VesselViewExportRecord, UsExpRailPermissions.ViewInboundRecord
        ],
        iconClass: 'icon-cloud-out'
      },
      {
        title: 'Audit',
        url: '/audit',
        permissions: [
          Permissions.AuditRailImportTrainConsistSheet,
          Permissions.AuditRailDailyAudit
        ],
        iconClass: 'icon-check',
        children: [
          {
            title: 'Rail - US',
            url: '/audit/rail',
            permissions: [Permissions.AuditRailImportTrainConsistSheet,
            Permissions.AuditRailDailyAudit],
            iconClass: 'icon-rail'
          },
        ]
      },
      {
        title: 'Recon',
        url: '/recon',
        permissions: [
          ReconPermissions.ViewInboundRecord,
        ],
        iconClass: 'icon-check',
        children: [
          {
            title: 'Report',
            url: '/recon',
            permissions: [ReconPermissions.ViewInboundRecord],
            iconClass: 'icon-report'
          },
        ]
      },
      {
        title: 'Zones',
        url: '/zones',
        children: [
          {
            title: 'In Bond',
            url: '/zones/in-bond',
            permissions: [
              ZonesInBondPermissions.ViewInboundRecord
            ],
            iconClass: 'icon-cloud-out'
          },
          {
            title: 'Entry 06',
            url: '/zones/entry-06',
            permissions: [
              ZonesEntryPermissions.ViewInboundRecord
            ],
            iconClass: 'icon-cloud-in'
          },
          {
            title: 'FTZ 214',
            url: '/zones/ftz-214',
            permissions: [
              ZonesFtz214Permissions.ViewInboundRecord
            ],
            iconClass: 'icon-cloud-in'
          }
        ],
        permissions: [
          ZonesInBondPermissions.ViewInboundRecord, ZonesEntryPermissions.ViewInboundRecord, ZonesFtz214Permissions.ViewInboundRecord
        ],
        iconClass: 'icon-cloud-out'
      },
      {
        title: 'ISF',
        url: '/isf',
        children: [
          {
            title: 'ISF',
            url: '/isf/inbound',
            permissions: [
              IsfPermissions.ViewInboundRecord
            ],
            iconClass: 'icon-cloud-out'
          }
        ],
        permissions: [
          IsfPermissions.ViewInboundRecord
        ],
        iconClass: 'icon-cloud-out'
      },
      {
        title: 'Rules Engine',
        url: '/rules',
        children: [
          {
            title: 'Rail - US',
            url: '/rules/rail',
            permissions: [Permissions.RailViewInboundRecordRules],
            iconClass: 'icon-rail'
          },
          {
            title: 'Pipeline - US',
            url: '/rules/pipeline',
            permissions: [Permissions.PipelineViewInboundRecordRules],
            iconClass: 'icon-pipeline'
          },
          {
            title: 'Truck - US',
            url: '/rules/truck',
            permissions: [Permissions.TruckViewInboundRecordRules, Permissions.TruckViewExportRecordRules],
            iconClass: 'icon-truck'
          },
          {
            title: 'Vessel - US',
            url: '/rules/vessel',
            permissions: [Permissions.VesselViewImportRecordRules, Permissions.VesselViewExportRecordRules],
            iconClass: 'icon-vessel'
          },
          {
            title: 'In Bond - US',
            url: '/rules/zones-inbond',
            permissions: [ZonesInBondPermissions.ViewRules],
            iconClass: 'icon-cloud-out'
          },
          {
            title: 'Entry 06 - US',
            url: '/rules/zones-entry-06',
            permissions: [ZonesEntryPermissions.ViewRules],
            iconClass: 'icon-cloud-out'
          },
          // {
          //   title: 'FTZ 214 - US',
          //   url: '/rules/zones-ftz-214',
          //   permissions: [ZonesFtz214Permissions.ViewRules],
          //   iconClass: 'icon-cloud-out'
          // },
          {
            title: 'Truck - CA',
            url: '/rules/canada-imp-truck',
            permissions: [CanadaImpTruckPermissions.ViewRules],
            iconClass: 'icon-truck'
          },
        ],
        permissions: [
          Permissions.RailViewInboundRecordRules,
          Permissions.PipelineViewInboundRecordRules,
          Permissions.TruckViewInboundRecordRules,
          Permissions.TruckViewExportRecordRules,
          Permissions.VesselViewImportRecordRules,
          Permissions.VesselViewExportRecordRules,
          ZonesInBondPermissions.ViewRules,
          ZonesEntryPermissions.ViewRules,
          // ZonesFtz214Permissions.ViewRules,
          CanadaImpTruckPermissions.ViewRules,
          UsExpRailPermissions.ViewRules,
          ZonesEntryPermissions.ViewRules
        ],
        iconClass: 'icon-rules'
      },
      {
        title: 'Administration',
        url: '/admin',
        children: [
          {
            title: 'Configuration',
            url: '/admin/rules-configuration',
            permissions: [
              Permissions.ViewConfiguration,
              Permissions.EditConfiguration
            ],
            iconClass: 'icon-rules'
          },
          {
            title: 'Client Management',
            url: '/admin/client-management',
            permissions: [Permissions.ViewClients],
            iconClass: 'icon-user-tie'
          },
          {
            title: 'Auto-create config',
            url: '/admin/auto-create',
            permissions: [Permissions.AdminAutoCreateConfiguration],
            iconClass: 'icon-rules'
          }
        ],
        permissions: [
          Permissions.ViewConfiguration,
          Permissions.EditConfiguration,
          Permissions.ViewClients,
          Permissions.AdminAutoCreateConfiguration
        ],
        iconClass: 'icon-settings'
      }
    ];
  }
}
