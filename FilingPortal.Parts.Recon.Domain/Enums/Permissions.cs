using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Defines the Recon Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "ReconPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View Recon Records Permission")]
        ViewInboundRecord = 24001,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import Recon Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to Export Records
        /// </summary>
        [Description("Export Recon Records Permission")]
        ExportInboundRecord,

        /// <summary>
        /// Defines the permission to View W Records
        /// </summary>
        [Description("View FTA Recon Records Permission")]
        ViewFtaRecord = 24011,

        /// <summary>
        /// Defines the permission to Import FTA Recon Records
        /// </summary>
        [Description("Import FTA Recon Records Permission")]
        ImportFtaRecord,

        /// <summary>
        /// Defines the permission to Export FTA Recon Records
        /// </summary>
        [Description("Export FTA Recon Records Permission")]
        ExportFtaRecord,

        /// <summary>
        /// Defines the permission to Process FTA Recon Records
        /// </summary>
        [Description("Process FTA Recon Records Permission")]
        ProcessFtaRecord,
        
        /// <summary>
        /// Defines the permission to View Value Records
        /// </summary>
        [Description("View Value Recon Records Permission")]
        ViewValueRecord = 24021,

        /// <summary>
        /// Defines the permission to Import Value Recon Records
        /// </summary>
        [Description("Import Value Recon Records Permission")]
        ImportValueRecord,

        /// <summary>
        /// Defines the permission to Export Value Recon Records
        /// </summary>
        [Description("Export Value Recon Records Permission")]
        ExportValueRecord,

        /// <summary>
        /// Defines the permission to Process Value Recon Records
        /// </summary>
        [Description("Process Value Recon Records Permission")]
        ProcessValueRecord,
    }
}
