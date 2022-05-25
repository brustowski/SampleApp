using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Rail.Export.Domain.Enums
{
    /// <summary>
    /// Defines the US Rail Export Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "UsExpRailPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View US Rail Export Records Permission")]
        ViewInboundRecord = 23001,

        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete US Rail Export Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File US Rail Export Records Permission")]
        FileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import US Rail Export Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Rule Records
        /// </summary>
        [Description("View US Rail Export Rules Permission")]
        ViewRules,

        /// <summary>
        /// Defines the permission to Edit Rule Records
        /// </summary>
        [Description("Edit US Rail Export Rules Permission")]
        EditRules,

        /// <summary>
        /// Defines the permission to Delete Rule Records
        /// </summary>
        [Description("Delete US Rail Export Rules Permission")]
        DeleteRules,
    }
}
