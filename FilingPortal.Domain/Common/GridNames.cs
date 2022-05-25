using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Common
{
    /// <summary>
    /// Class representing constants of the existing grid names
    /// </summary>
    [TsClass(IncludeNamespace = false)]
    public class GridNames
    {
        #region Rail

        /// <summary>
        /// The inbound records grid name definition
        /// </summary>
        [TsProperty]
        public const string InboundRecords = "inbound_records";

        /// <summary>
        /// The inbound records Unique Data grid name definition
        /// </summary>
        [TsProperty]
        public const string InboundRecordsUniqueData = "inbound_records_unique_data";

        /// <summary>
        /// The Rail Importer-Supplier Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string RailRuleImporterSupplier = "rail_rule_importer_supplier";

        /// <summary>
        /// The Rail Description Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string RailRuleDescription = "rail_rule_description";

        /// <summary>
        /// The Rail Port Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string RailRulePort = "rail_rule_port";

        /// <summary>
        /// The Rail Default Values records grid name definition
        /// </summary>
        [TsProperty]
        public const string RailDefaultValues = "rail_default_values";

        /// <summary>
        /// Single-filing grid name
        /// </summary>
        [TsProperty]
        public const string RailSingleFilingGrid = "rail_single_filing";

        /// <summary>
        /// Consolidated filing manifest data grid name
        /// </summary>
        [TsProperty]
        public const string RailManifestDataGrid = "rail_manifest_data";

        #endregion

        #region Pipeline

        /// <summary>
        /// The Pipeline Inbound records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineInboundRecords = "pipeline_inbound_records";

        /// <summary>
        /// The Pipeline Rule Batch Code records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineRuleBatchCode = "pipeline_rule_batch_code";

        /// <summary>
        /// The Pipeline Rule Importer records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineRuleImporter = "pipeline_rule_importer";


        /// <summary>
        /// The Pipeline Rule Facility records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineRuleFacility = "pipeline_rule_facility";

        /// <summary>
        /// The Default Values records filter name definition
        /// </summary>
        [TsProperty]
        public const string PipelineDefaultValues = "pipeline_default_values";

        /// <summary>
        /// The Pipeline Inbound Unique Data records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineInboundUniqueDataGrid = "pipeline_inbound_records_unique_data";

        /// <summary>
        /// Pipeline Single-filing grid name
        /// </summary>
        [TsProperty]
        public const string PipelineSingleFilingGrid = "pipeline_single_filing";

        /// <summary>
        /// Pipeline consignee-importer rule grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineRuleConsigneeImporter = "pipeline_rule_consignee_importer";
        /// <summary>
        /// The Pipeline Rule Price records grid name definition
        /// </summary>
        [TsProperty]
        public const string PipelineRulePrice = "pipeline_rule_price";

        #endregion

        #region Truck

        /// <summary>
        /// The Truck Inbound records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckInboundRecords = "truck_inbound_records";

        /// <summary>
        /// The Truck Importer Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckRuleImporter = "truck_rule_importer";

        /// <summary>
        /// The Truck Port Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckRulePort = "truck_rule_port";

        /// <summary>
        /// The Truck Inbound Unique Data records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckInboundUniqueDataGrid = "truck_inbound_records_unique_data";

        /// <summary>
        /// The Truck Default Values records grid name
        /// </summary>
        [TsProperty]
        public const string TruckDefaultValues = "truck_default_values";

        /// <summary>
        /// Truck Single-filing grid name
        /// </summary>
        [TsProperty]
        public const string TruckSingleFilingGrid = "truck_single_filing";
        #endregion

        #region TruckExport

        /// <summary>
        /// The Truck Export Consignee Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckExportRuleConsignee = "truck_export_rule_consignee";

        /// <summary>
        /// The Truck Export Exporter-Consignee Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckExportRuleExporterConsignee = "truck_export_rule_exporter_consignee";
        /// <summary>
        /// The Truck Export records grid name definition
        /// </summary>
        [TsProperty]
        public const string TruckExportRecords = "truck_export";
        /// <summary>
        /// The Truck Export Default Values records grid name
        /// </summary>
        [TsProperty]
        public const string TruckExportDefaultValues = "truck_export_default_values";
        /// <summary>
        /// The Truck Export Single-filing gird name 
        /// </summary>
        [TsProperty]
        public const string TruckExportSingleFilingGrid = "truck_export_single_filing";
        #endregion

        #region Vessel

        /// <summary>
        /// Vessel Importer Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselRuleImporter = "vessel_rule_importer";

        /// <summary>
        /// Vessel Port Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselRulePort = "vessel_rule_port";

        /// <summary>
        /// Vessel Product Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselRuleProduct = "vessel_rule_product";

        /// <summary>
        /// The Vessel Import records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselImportRecords = "vessel_import_records";

        /// <summary>
        /// The Vessel Default Values records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselDefaultValues = "vessel_default_values";
        /// <summary>
        /// Single-filing grid name
        /// </summary>
        [TsProperty]
        public const string VesselSingleFilingGrid = "vessel_single_filing";

        #endregion

        #region VersselExport

        /// <summary>
        /// The Vessel Export Usppi-Consignee Rule records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselExportRuleUsppiConsignee = "vessel_export_rule_usppi_consignee";
        /// <summary>
        /// The Vessel Export records grid name definition
        /// </summary>
        [TsProperty]
        public const string VesselExportRecords = "vessel_export";
        /// <summary>
        /// The Vessel Export Default Values records grid name
        /// </summary>
        [TsProperty]
        public const string VesselExportDefaultValues = "vessel_export_default_values";
        /// <summary>
        /// The Vessel Export Single-filing gird name 
        /// </summary>
        [TsProperty]
        public const string VesselExportSingleFilingGrid = "vessel_export_single_filing";
        #endregion


        #region Common
        /// <summary>
        /// The Clients records grid name definition
        /// </summary>
        [TsProperty]
        public const string Clients = "clients";

        #endregion

        #region Audit
        /// <summary>
        /// Rail Audit - Train Consist Sheet
        /// </summary>
        [TsProperty]
        public const string AuditRailTrainConsistSheet = "rail_train_consist_sheet";
        /// <summary>
        /// Rail Audit - Daily Audit
        /// </summary>
        [TsProperty]
        public const string AuditRailDailyAudit = "rail_daily_audit";
        /// <summary>
        /// Rail Audit - Daily Audit Rules
        /// </summary>
        [TsProperty]
        public const string AuditRailDailyAuditRules = "rail_daily_audit_rules";
        /// <summary>
        /// Rail Audit - Daily Audit SPI Rules
        /// </summary>
        [TsProperty]
        public const string AuditRailDailyAuditSpiRules = "rail_daily_audit_spi_rules";
        #endregion

        #region Admin
        /// <summary>
        /// The Auto-create records grid name definition
        /// </summary>
        [TsProperty]
        public const string AutoCreateRecords = "admin_auto_create_records";
        #endregion
    }
}