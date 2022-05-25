using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.Enums
{
    /// <summary>
    /// Defines the Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false)]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Rail Inbound Records
        /// </summary>
        [Description("View Rail Inbound Records Permission")]
        RailViewInboundRecord = 1,

        /// <summary>
        /// Defines the permission to Delete Rail Inbound Records
        /// </summary>
        [Description("Delete Rail Inbound Records Permission")]
        RailDeleteInboundRecord,

        /// <summary>
        /// Defines the permission to View Rail Inbound Records Manifest
        /// </summary>
        [Description("View Rail Inbound Records Manifest Permission")]
        RailViewManifest,

        /// <summary>
        /// Defines the permission to File Rail Inbound Records
        /// </summary>
        [Description("File Rail Inbound Records Permission")]
        RailFileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Pipeline Inbound Records
        /// </summary>
        [Description("Import Pipeline Inbound Records Permission")]
        PipelineImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Pipeline Inbound Records
        /// </summary>
        [Description("View Pipeline Inbound Records Permission")]
        PipelineViewInboundRecord,

        /// <summary>
        /// Defines the permission to Delete Pipeline Inbound Records
        /// </summary>
        [Description("Delete Pipeline Inbound Records Permission")]
        PipelineDeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Pipeline Inbound Records
        /// </summary>
        [Description("File Pipeline Inbound Records Permission")]
        PipelineFileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Truck Inbound Records
        /// </summary>
        [Description("Import Truck Inbound Records Permission")]
        TruckImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Truck Inbound Records
        /// </summary>
        [Description("View Truck Inbound Records Permission")]
        TruckViewInboundRecord,

        /// <summary>
        /// Defines the permission to Delete Truck Inbound Records
        /// </summary>
        [Description("Delete Truck Inbound Records Permission")]
        TruckDeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Truck Inbound Records
        /// </summary>
        [Description("File Truck Inbound Records Permission")]
        TruckFileInboundRecord,

        /// <summary>
        /// Defines the permission to View Client Records
        /// </summary>
        [Description("View Client Records Permission")]
        ViewClients,

        /// <summary>
        /// Defines the permission to View Rail Inbound Rule Records
        /// </summary>
        [Description("View Rail Inbound Rule Records Permission")]
        RailViewInboundRecordRules,

        /// <summary>
        /// Defines the permission to Edit Rail Inbound Rule Records
        /// </summary>
        [Description("Edit Rail Inbound Rule Records Permission")]
        RailEditInboundRecordRules,

        /// <summary>
        /// Defines the permission to Delete Rail Inbound Rule Records
        /// </summary>
        [Description("Delete Rail Inbound Rule Records Permission")]
        RailDeleteInboundRecordRules,

        /// <summary>
        /// Defines the permission to View Pipeline Inbound Rule Records
        /// </summary>
        [Description("View Pipeline Inbound Rule Records Permission")]
        PipelineViewInboundRecordRules,

        /// <summary>
        /// Defines the permission to Edit Pipeline Inbound Rule Records
        /// </summary>
        [Description("Edit Pipeline Inbound Rule Records Permission")]
        PipelineEditInboundRecordRules,

        /// <summary>
        /// Defines the permission to Delete Pipeline Inbound Rule Records
        /// </summary>
        [Description("Delete Pipeline Inbound Rule Records Permission")]
        PipelineDeleteInboundRecordRules,

        /// <summary>
        /// Defines the permission to View Truck Inbound Rule Records
        /// </summary>
        [Description("View Truck Inbound Rule Records Permission")]
        TruckViewInboundRecordRules,

        /// <summary>
        /// Defines the permission to Edit Truck Inbound Rule Records
        /// </summary>
        [Description("Edit Truck Inbound Rule Records Permission")]
        TruckEditInboundRecordRules,

        /// <summary>
        /// Defines the permission to Delete Truck Inbound Rule Records
        /// </summary>
        [Description("Delete Truck Inbound Rule Records Permission")]
        TruckDeleteInboundRecordRules,

        /// <summary>
        /// Defines the permission to View Configuration Records
        /// </summary>
        [Description("View Configuration Records Permission")]
        ViewConfiguration,

        /// <summary>
        /// Defines the permission to Edit Configuration Records
        /// </summary>
        [Description("Edit Configuration Records Permission")]
        EditConfiguration,

        /// <summary>
        /// Defines the permission to Delete Configuration Records
        /// </summary>
        [Description("Delete Configuration Records Permission")]
        DeleteConfiguration,

        /// <summary>
        /// Defines the permission to view and start Single-filing
        /// </summary>
        [Description("Execution of Single Filing process permission")]
        ExecuteSingleFiling,

        /// <summary>
        /// Defines the permission to View Vessel Import Rule Records
        /// </summary>
        [Description("View Vessel Import Rule Records Permission")]
        VesselViewImportRecordRules,

        /// <summary>
        /// Defines the permission to Edit Vessel Import Rule Records
        /// </summary>
        [Description("Edit Vessel Import Rule Records Permission")]
        VesselEditImportRecordRules,

        /// <summary>
        /// Defines the permission to Delete Vessel Import Rule Records
        /// </summary>
        [Description("Delete Vessel Import Rule Records Permission")]
        VesselDeleteImportRecordRules,

        /// <summary>
        /// Defines the permission to View Vessel Inbound Records
        /// </summary>
        [Description("View Vessel Import Records Permission")]
        VesselViewImportRecord,

        /// <summary>
        /// Defines the permission to Delete Vessel Inbound Records
        /// </summary>
        [Description("Delete Vessel Import Records Permission")]
        VesselDeleteImportRecord,

        /// <summary>
        /// Defines the permission to File Vessel Inbound Records
        /// </summary>
        [Description("File Vessel Import Records Permission")]
        VesselFileImportRecord,
        /// <summary>
        /// Defines the permission to Add Vessel Inbound Records
        /// </summary>
        [Description("Add Vessel Import Records Permission")]
        VesselAddImportRecord,

        /// <summary>
        /// Defines the permission to Import Truck Export Records
        /// </summary>
        [Description("Import Truck Export Records Permission")]
        TruckImportExportRecord = 34,

        /// <summary>
        /// Defines the permission to View Truck Export Records
        /// </summary>
        [Description("View Truck Export Records Permission")]
        TruckViewExportRecord,

        /// <summary>
        /// Defines the permission to Delete Truck Export Records
        /// </summary>
        [Description("Delete Truck Export Records Permission")]
        TruckDeleteExportRecord,

        /// <summary>
        /// Defines the permission to File Truck Export Records
        /// </summary>
        [Description("File Truck Export Records Permission")]
        TruckFileExportRecord,

        /// <summary>
        /// Defines the permission to View Truck Export Rule Records
        /// </summary>
        [Description("View Truck Export Rule Records Permission")]
        TruckViewExportRecordRules,

        /// <summary>
        /// Defines the permission to Edit Truck Export Rule Records
        /// </summary>
        [Description("Edit Truck Export Rule Records Permission")]
        TruckEditExportRecordRules,

        /// <summary>
        /// Defines the permission to Delete Truck Export Rule Records
        /// </summary>
        [Description("Delete Truck Export Rule Records Permission")]
        TruckDeleteExportRecordRules,
        /// <summary>
        /// Defines the permission to Add Vessel Export Records
        /// </summary>
        [Description("Add Vessel Export Records Permission")]
        VesselAddExportRecord,

        /// <summary>
        /// Defines the permission to View Vessel Export Records
        /// </summary>
        [Description("View Vessel Export Records Permission")]
        VesselViewExportRecord,

        /// <summary>
        /// Defines the permission to Delete Vessel Export Records
        /// </summary>
        [Description("Delete Vessel Export Records Permission")]
        VesselDeleteExportRecord,

        /// <summary>
        /// Defines the permission to File Vessel Export Records
        /// </summary>
        [Description("File Vessel Export Records Permission")]
        VesselFileExportRecord,

        /// <summary>
        /// Defines the permission to View Vessel Export Rule Records
        /// </summary>
        [Description("View Vessel Export Rule Records Permission")]
        VesselViewExportRecordRules,

        /// <summary>
        /// Defines the permission to Edit Vessel Export Rule Records
        /// </summary>
        [Description("Edit Vessel Export Rule Records Permission")]
        VesselEditExportRecordRules,

        /// <summary>
        /// Defines the permission to Delete Vessel Export Rule Records
        /// </summary>
        [Description("Delete Vessel Export Rule Records Permission")]
        VesselDeleteExportRecordRules,

        #region Audit

        #region Rail

        /// <summary>
        /// Defines the permission to import train consist sheet to System
        /// </summary>
        [Description("Audit - Import Train Consist Sheet")]
        AuditRailImportTrainConsistSheet,
        /// <summary>
        /// Defines the permission to work with daily audit
        /// </summary>
        [Description("Working with rail daily audit")]
        AuditRailDailyAudit,

        #endregion
        #endregion

        #region Admin
        [Description("Work with Auto-Create configurations")]
        AdminAutoCreateConfiguration
        #endregion
    }
}
