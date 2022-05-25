using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Entry.Domain.Enums
{
    /// <summary>
    /// Defines the Zones Entry Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "ZonesEntryPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View Zones Entry Records Permission")]
        ViewInboundRecord = 21100,

        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete Zones Entry Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File Zones Entry Records Permission")]
        FileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import Zones Entry Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Rule Records
        /// </summary>
        [Description("View Zones Entry Rules Permission")]
        ViewRules,

        /// <summary>
        /// Defines the permission to Edit Rule Records
        /// </summary>
        [Description("Edit Zones Entry Rules Permission")]
        EditRules,

        /// <summary>
        /// Defines the permission to Delete Rule Records
        /// </summary>
        [Description("Delete Zones Entry Rules Permission")]
        DeleteRules,

    }
}
