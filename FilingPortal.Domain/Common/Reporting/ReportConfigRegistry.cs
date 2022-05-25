using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Services.GridExport.Models;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;
using FilingPortal.Domain.Services.GridExport.Models.Pipeline;
using FilingPortal.Domain.Services.GridExport.Models.Vessel;
using FilingPortal.Domain.Services.GridExport.Models.VesselExport;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Implements report configuration registry
    /// </summary>
    public class ReportConfigRegistry : IReportConfigRegistry
    {
        /// <summary>
        /// Report configurations dictionary
        /// </summary>
        private readonly IDictionary<string, IReportConfig> _reportConfigsDictionary;

        /// <summary>
        /// Creates a new instance of <see cref="ReportConfigRegistry"/>
        /// </summary>
        public ReportConfigRegistry(IEnumerable<IReportConfig> configsRegistry)
        {
            var localReportConfig = new IReportConfig[]
            {
                new ReportConfig<RailInboundRecordsReportModel>(GridNames.InboundRecords) {FileName = "Rail_import.xlsx",DocumentTitle = "RailImports"},
                new ReportConfig<PipelineInboundReadModel>(GridNames.PipelineInboundRecords) {FileName = "Pipeline_import.xlsx", DocumentTitle = "PipelineImports"},
                new ReportConfig<TruckInboundReadModel>(GridNames.TruckInboundRecords) {FileName = "Truck_import.xlsx", DocumentTitle = "TruckImport"},
                new ReportConfig<TruckExportReadModel>(GridNames.TruckExportRecords) {FileName = "Truck_export.xlsx", DocumentTitle = "TruckExports"},
                new ReportConfig<VesselImportReadModel>(GridNames.VesselImportRecords) {FileName = "Vessel_imports.xlsx", DocumentTitle = "VesselImports"},
                new ReportConfig<VesselExportReadModel>(GridNames.VesselExportRecords) {FileName = "Vessel_exports.xlsx", DocumentTitle = "VesselExports"},
                // Rules
                new ReportConfig<RailRuleImporterSupplier>(GridNames.RailRuleImporterSupplier) {FileName = "RailRuleImporterSupplier.xlsx", DocumentTitle = "RailRuleImporterSupplier"},
                new ReportConfig<RailRuleDescription>(GridNames.RailRuleDescription) {FileName = "RailRuleDescription.xlsx", DocumentTitle = "RailRuleDescription"},
                new ReportConfig<RailRulePort>(GridNames.RailRulePort) {FileName = "RailRulePort.xlsx", DocumentTitle = "RailRulePort"},
                // Pipeline
                new ReportConfig<PipelineRuleImporter>(GridNames.PipelineRuleImporter) {FileName = "PipelineRuleImporter.xlsx", DocumentTitle = "PipelineRuleImporter"},
                new ReportConfig<PipelineRuleBatchCode>(GridNames.PipelineRuleBatchCode) {FileName = "PipelineRuleBatchCode.xlsx", DocumentTitle = "PipelineRuleBatchCode"},
                new ReportConfig<PipelineRuleFacility>(GridNames.PipelineRuleFacility) {FileName = "PipelineRuleFacility.xlsx", DocumentTitle = "PipelineRuleFacility"},
                new ReportConfig<PipelineRuleConsigneeImporter>(GridNames.PipelineRuleConsigneeImporter) {FileName = "PipelineRuleConsigneeImporter.xlsx", DocumentTitle = "PipelineRuleConsigneeImporter"},
                new ReportConfig<PipelineRulePriceReportModel>(GridNames.PipelineRulePrice) {FileName = "PipelineRulePrice.xlsx", DocumentTitle = "PipelineRulePrice"},
                // Truck
                new ReportConfig<TruckRuleImporter>(GridNames.TruckRuleImporter) {FileName = "TruckRuleImporter.xlsx", DocumentTitle = "TruckRuleImporter"},
                new ReportConfig<TruckRulePort>(GridNames.TruckRulePort) {FileName = "TruckRulePort.xlsx", DocumentTitle = "TruckRulePort"},
                new ReportConfig<TruckExportRuleConsignee>(GridNames.TruckExportRuleConsignee) {FileName = "TruckExportRuleConsignee.xlsx", DocumentTitle = "TruckExportRuleConsignee"},
                new ReportConfig<TruckExportRuleExporterConsignee>(GridNames.TruckExportRuleExporterConsignee) {FileName = "TruckExportRuleExporterConsignee.xlsx", DocumentTitle = "TruckExportRuleExporterConsignee"},
                // Vessel
                new ReportConfig<VesselRulePortsReportModel>(GridNames.VesselRulePort) {FileName = "VesselRulePort.xlsx", DocumentTitle = "VesselRulePort"},
                new ReportConfig<VesselRuleProduct>(GridNames.VesselRuleProduct) {FileName = "VesselRuleProduct.xlsx", DocumentTitle = "VesselRuleProduct"},
                new ReportConfig<VesselExportUsppiConsigneeReportModel>(GridNames.VesselExportRuleUsppiConsignee) {FileName = "VesselExportRuleUsppiConsignee.xlsx", DocumentTitle = "VesselExportRuleUsppiConsignee"},
                // Audit
                // Rail
                new ReportConfig<AuditRailTrainConsistSheetReportModel>(GridNames.AuditRailTrainConsistSheet) {FileName = "AuditRailTrainConsistSheet.xlsx",DocumentTitle = "AuditRailTrainConsistSheet"},
                new ReportConfig<AuditRailDailyAuditReportModel>(GridNames.AuditRailDailyAudit) {FileName = "AuditRailDailyAudit.xlsx",DocumentTitle = "AuditRailDailyAudit"},
                new ReportConfig<AuditRailDailyAuditRulesReportModel>(GridNames.AuditRailDailyAuditRules) {FileName = "AuditRailDailyAuditRules.xlsx",DocumentTitle = "AuditRailDailyAuditRules"},
                new ReportConfig<AuditRailDailyAuditSpiRulesReportModel>(GridNames.AuditRailDailyAuditSpiRules) {FileName = "AuditRailDailyAuditSpiRules.xlsx",DocumentTitle = "AuditRailDailyAuditSpiRules"},
                // Admin
                new ReportConfig<AutoCreateRecord>(GridNames.AutoCreateRecords) {FileName = "AutoCreateRecords.xlsx", DocumentTitle = "AutoCreateRecords"},
            };

            _reportConfigsDictionary = localReportConfig.Union(configsRegistry).ToDictionary(rc => rc.Name);
        }
        /// <summary>
        /// Gets report configuration
        /// </summary>
        /// <param name="gridName">Grid name</param>
        public IReportConfig GetConfig(string gridName)
        {
            var key = gridName;
            if (!_reportConfigsDictionary.ContainsKey(key))
                throw new KeyNotFoundException($"A report config for the grid name '{key}' was not found in the report config registry.");
            return _reportConfigsDictionary[key];
        }
    }
}
