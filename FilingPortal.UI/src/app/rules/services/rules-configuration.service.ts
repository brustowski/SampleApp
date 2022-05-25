import { Injectable } from '@angular/core';
import { NavigationTabConfig } from '@common/navigation-tabs';
import { Page } from '@common/grid/models/page';
import { PageBuilder } from '@common/grid/models';
import { PageConfigNames, GridNames, Permissions, UsExpRailGridNames, UsExpRailPageConfigNames } from '@common/models';
import { IGridsConfigurationService } from '@common/interfaces/grids-configuration-service';
import {
  CanadaImpTruckPermissions, CanadaImpTruckGridNames, CanadaImpTruckPageConfigNames,
  ZonesInBondPermissions, ZonesInBondGridNames, ZonesInBondPageConfigNames, UsExpRailPermissions
} from '@common/models';
import { ZonesEntryPermissions, ZonesEntry06GridNames, ZonesEntry06PageConfigNames } from '@common/models/zones-entry-models';

interface Map<T> {
  [key: string]: T;
}

@Injectable()
export class RulesConfigurationService implements IGridsConfigurationService {
  private tabs: Map<NavigationTabConfig> = {};
  private grids: Map<Page> = {};
  private pageActions: Map<string> = {};

  constructor() {
    this.initTabs();
    this.initGrids();
    this.initPageActions();
  }

  private initTabs(): void {
    this.tabs['rail'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'rail-importer-supplier', title: 'Importer Supplier', permissions: [Permissions.RailViewInboundRecordRules] },
        { url: 'rail-descriptions', title: 'Product Description', permissions: [Permissions.RailViewInboundRecordRules] },
        { url: 'rail-ports', title: 'Ports', permissions: [Permissions.RailViewInboundRecordRules] },
        { url: 'rail-export-consignee', title: '(Export) Consignee', permissions: [UsExpRailPermissions.ViewRules] },
        {
          url: 'rail-export-exporter-consignee', title: '(Export) USPPI-Consignee',
          permissions: [UsExpRailPermissions.ViewRules]
        }
      ]
    };

    this.tabs['pipeline'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'pipeline-importer', title: 'Importer', permissions: [Permissions.PipelineViewInboundRecordRules] }
        , { url: 'pipeline-batch', title: 'Batch', permissions: [Permissions.PipelineViewInboundRecordRules] }
        , { url: 'pipeline-facility', title: 'To Facility', permissions: [Permissions.PipelineViewInboundRecordRules] }
        , { url: 'pipeline-consignee-importer', title: 'Consignee-Importer', permissions: [Permissions.PipelineViewInboundRecordRules] }
        , { url: 'pipeline-price', title: 'Price', permissions: [Permissions.PipelineViewInboundRecordRules] }
      ]
    };

    this.tabs['truck'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'truck-importer', title: 'Importer', permissions: [Permissions.TruckViewInboundRecordRules] },
        { url: 'truck-ports', title: 'Ports', permissions: [Permissions.TruckViewInboundRecordRules] },
        { url: 'truck-export-consignee', title: '(Export) Consignee', permissions: [Permissions.TruckViewExportRecordRules] },
        {
          url: 'truck-export-exporter-consignee', title: '(Export) USPPI-Consignee',
          permissions: [Permissions.TruckViewExportRecordRules]
        }
      ]
    };

    this.tabs['vessel'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'vessel-ports', title: 'Ports', permissions: [Permissions.VesselViewImportRecordRules] },
        { url: 'vessel-products', title: 'Products', permissions: [Permissions.VesselViewImportRecordRules] },
        { url: 'vessel-export-usppi-consignee', title: '(Export) USPPI-Consignee', permissions: [Permissions.VesselViewExportRecordRules] }
      ]
    };

    this.tabs['zones-inbond'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'zones-inbond-entry', title: 'Entry', permissions: [ZonesInBondPermissions.ViewRules] },
      ]
    };

    this.tabs['zones-entry-06'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'zones-entry-06-importer', title: 'Importer', permissions: [ZonesEntryPermissions.ViewRules] },
      ]
    };

    this.tabs['canada-imp-truck'] = {
      cssClass: 'rules-tabs',
      tabs: [
        { url: 'canada-imp-truck-vendor', title: 'Vendor', permissions: [CanadaImpTruckPermissions.ViewRules] },
        { url: 'canada-imp-truck-port', title: 'Port', permissions: [CanadaImpTruckPermissions.ViewRules] },
        { url: 'canada-imp-truck-product', title: 'Product', permissions: [CanadaImpTruckPermissions.ViewRules] },
      ]
    };
  }

  private initGrids(): void {
    const builder = new PageBuilder();
    this.grids['rail-importer-supplier'] = builder
      .create()
      .title('Rail Importer Supplier Rule')
      .pathForApi('rules/rail/importer-supplier')
      .gridName(GridNames.RailRuleImporterSupplier)
      .filterConfigName(GridNames.RailRuleImporterSupplier)
      .build();
    this.grids['rail-descriptions'] = builder
      .create()
      .title('Rail Descriptions Rule')
      .pathForApi('rules/rail/description')
      .gridName(GridNames.RailRuleDescription)
      .filterConfigName(GridNames.RailRuleDescription)
      .build();
    this.grids['rail-ports'] = builder
      .create()
      .title('Rail Ports Rule')
      .pathForApi('rules/rail/port')
      .gridName(GridNames.RailRulePort)
      .filterConfigName(GridNames.RailRulePort)
      .build();

    this.grids['pipeline-importer'] = builder.create().title('Pipeline Importer Rule').pathForApi('rules/pipeline/importer')
      .gridName(GridNames.PipelineRuleImporter).filterConfigName(GridNames.PipelineRuleImporter).build();
    this.grids['pipeline-batch'] = builder.create().title('Pipeline Batch Rule').pathForApi('rules/pipeline/batch-code')
      .gridName(GridNames.PipelineRuleBatchCode).filterConfigName(GridNames.PipelineRuleBatchCode).build();
    this.grids['pipeline-facility'] = builder.create().title('Pipeline Facility Rule').pathForApi('rules/pipeline/facility')
      .gridName(GridNames.PipelineRuleFacility).filterConfigName(GridNames.PipelineRuleFacility).build();
    this.grids['pipeline-consignee-importer'] = builder.create().title('Pipeline Consignee-Importer Rule')
      .pathForApi('rules/pipeline/consignee-importer').gridName(GridNames.PipelineRuleConsigneeImporter)
      .filterConfigName(GridNames.PipelineRuleConsigneeImporter).build();
    this.grids['pipeline-price'] = builder.create().title('Pipeline Price Rule')
      .pathForApi('rules/pipeline/price').gridName(GridNames.PipelineRulePrice)
      .filterConfigName(GridNames.PipelineRulePrice).build();

    this.grids['truck-importer'] = builder
      .create()
      .title('Truck Importer Rule')
      .pathForApi('rules/truck/importer')
      .gridName(GridNames.TruckRuleImporter)
      .filterConfigName(GridNames.TruckRuleImporter)
      .build();
    this.grids['truck-ports'] = builder
      .create()
      .title('Truck Ports Rule')
      .pathForApi('rules/truck/port')
      .gridName(GridNames.TruckRulePort)
      .filterConfigName(GridNames.TruckRulePort)
      .build();

    this.grids['vessel-importer'] = builder
      .create()
      .title('Vessel Importer Rule')
      .pathForApi('rules/vessel/importer')
      .gridName(GridNames.VesselRuleImporter)
      .filterConfigName(GridNames.VesselRuleImporter)
      .build();
    this.grids['vessel-ports'] = builder
      .create()
      .title('Vessel Ports Rule')
      .pathForApi('rules/vessel/port')
      .gridName(GridNames.VesselRulePort)
      .filterConfigName(GridNames.VesselRulePort)
      .build();
    this.grids['vessel-products'] = builder
      .create()
      .title('Vessel Products Rule')
      .pathForApi('rules/vessel/product')
      .gridName(GridNames.VesselRuleProduct)
      .filterConfigName(GridNames.VesselRuleProduct)
      .build();

    this.grids['truck-export-consignee'] = builder
      .create()
      .title('Truck Export Consignee Rule')
      .pathForApi('rules/export/truck/consignee')
      .gridName(GridNames.TruckExportRuleConsignee)
      .filterConfigName(GridNames.TruckExportRuleConsignee)
      .build();
    this.grids['truck-export-exporter-consignee'] = builder
      .create()
      .title('Truck Export Exporter-Consignee Rule')
      .pathForApi('rules/export/truck/exporter-consignee')
      .gridName(GridNames.TruckExportRuleExporterConsignee)
      .filterConfigName(GridNames.TruckExportRuleExporterConsignee)
      .build();
    this.grids['vessel-export-usppi-consignee'] = builder
      .create()
      .title('Vessel Export USPPI-Consignee Rule')
      .pathForApi('rules/export/vessel/usppi-consignee')
      .gridName(GridNames.VesselExportRuleUsppiConsignee)
      .filterConfigName(GridNames.VesselExportRuleUsppiConsignee)
      .build();

    this.grids['zones-inbond-entry'] = builder
      .create()
      .title('Zones In-Bond Entry rule')
      .pathForApi('rules/zones/in-bond/entry')
      .gridName(ZonesInBondGridNames.RuleEntry)
      .filterConfigName(ZonesInBondGridNames.RuleEntry)
      .build();

      this.grids['zones-entry-06-importer'] = builder
      .create()
      .title('Zones Entry 06 Importer rule')
      .pathForApi('rules/zones/entry-06/importer')
      .gridName(ZonesEntry06GridNames.ImporterRuleGrid)
      .filterConfigName(ZonesEntry06GridNames.ImporterRuleGrid)
      .build();

    this.grids['canada-imp-truck-vendor'] = builder
      .create()
      .title('Canada Truck Imports Vendor rule')
      .pathForApi('rules/canada-imp-truck/vendor')
      .gridName(CanadaImpTruckGridNames.RuleVendor)
      .filterConfigName(CanadaImpTruckGridNames.RuleVendor)
      .build();
    this.grids['canada-imp-truck-port'] = builder
      .create()
      .title('Canada Truck Imports Port rule')
      .pathForApi('rules/canada-imp-truck/port')
      .gridName(CanadaImpTruckGridNames.RulePortRecords)
      .filterConfigName(CanadaImpTruckGridNames.RulePortRecords)
      .build();
    this.grids['canada-imp-truck-product'] = builder
      .create()
      .title('Canada Truck Imports Product rule')
      .pathForApi('rules/canada-imp-truck/product')
      .gridName(CanadaImpTruckGridNames.RuleProduct)
      .filterConfigName(CanadaImpTruckGridNames.RuleProduct)
      .build();
    this.grids['rail-export-consignee'] = builder
      .create()
      .title('Rail Export Consignee Rule')
      .pathForApi('rules/export/rail/consignee')
      .gridName(UsExpRailGridNames.RuleConsignee)
      .filterConfigName(UsExpRailGridNames.RuleConsignee)
      .build();
    this.grids['rail-export-exporter-consignee'] = builder
      .create()
      .title('Rail Export Exporter-Consignee Rule')
      .pathForApi('rules/export/rail/exporter-consignee')
      .gridName(UsExpRailGridNames.RuleExporterConsignee)
      .filterConfigName(UsExpRailGridNames.RuleExporterConsignee)
      .build();
  }

  private initPageActions(): void {
    this.pageActions['rail-importer-supplier'] = PageConfigNames.RailRulesPageActions;
    this.pageActions['rail-descriptions'] = PageConfigNames.RailRulesPageActions;
    this.pageActions['rail-ports'] = PageConfigNames.RailRulesPageActions;

    this.pageActions['pipeline-importer'] = PageConfigNames.PipelineRulesPageActions;
    this.pageActions['pipeline-batch'] = PageConfigNames.PipelineRulesPageActions;
    this.pageActions['pipeline-facility'] = PageConfigNames.PipelineRulesPageActions;
    this.pageActions['pipeline-consignee-importer'] = PageConfigNames.PipelineRulesPageActions;
    this.pageActions['pipeline-price'] = PageConfigNames.PipelineRulesPageActions;

    this.pageActions['truck-importer'] = PageConfigNames.TruckRulesPageActions;
    this.pageActions['truck-ports'] = PageConfigNames.TruckRulesPageActions;

    this.pageActions['truck-export-consignee'] = PageConfigNames.TruckExportRulesPageActions;

    this.pageActions['vessel-importer'] = PageConfigNames.VesselRulesPageActions;
    this.pageActions['vessel-ports'] = PageConfigNames.VesselRulesPageActions;
    this.pageActions['vessel-products'] = PageConfigNames.VesselRulesPageActions;

    this.pageActions['vessel-export-usppi-consignee'] = PageConfigNames.VesselExportRulesPageActions;

    // Zones In-Bond
    this.pageActions['zones-inbond-entry'] = ZonesInBondPageConfigNames.RulesPageActions;
    // Zones In-Bond
    this.pageActions['zones-entry-06-importer'] = ZonesEntry06PageConfigNames.RulesPageActions;

    // Canada Import Truck
    this.pageActions['canada-imp-truck-vendor'] = CanadaImpTruckPageConfigNames.RulesPageActions;
    this.pageActions['canada-imp-truck-port'] = CanadaImpTruckPageConfigNames.RulesPageActions;
    this.pageActions['canada-imp-truck-product'] = CanadaImpTruckPageConfigNames.RulesPageActions;

    this.pageActions['rail-export-consignee'] = UsExpRailPageConfigNames.RulesPageActions;
    this.pageActions['rail-export-exporter-consignee'] = UsExpRailPageConfigNames.RulesPageActions;
  }
  getTabsConfig(value: string): NavigationTabConfig {
    return this.tabs[value];
  }

  getGridConfig(value: string): Page {
    return this.grids[value];
  }

  getPageActionsConfig(value: string): string {
    return this.pageActions[value];
  }
}
