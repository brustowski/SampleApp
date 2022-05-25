using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Inbond.Domain.Enums
{
    /// <summary>
    /// Defines the Zones In-Bond Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "ZonesInBondPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View Zones In-Bond Records Permission")]
        ViewInboundRecord = 21000,

        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete Zones In-Bond Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File Zones In-Bond Records Permission")]
        FileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import Zones In-Bond Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Rule Records
        /// </summary>
        [Description("View Zones In-Bond Rules Permission")]
        ViewRules,

        /// <summary>
        /// Defines the permission to Edit Rule Records
        /// </summary>
        [Description("Edit Zones In-Bond Rules Permission")]
        EditRules,

        /// <summary>
        /// Defines the permission to Delete Rule Records
        /// </summary>
        [Description("Delete Zones In-Bond Rules Permission")]
        DeleteRules
    }
}
