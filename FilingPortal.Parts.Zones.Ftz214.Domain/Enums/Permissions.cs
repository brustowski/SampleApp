using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Enums
{
    /// <summary>
    /// Defines the Zones Ftz214 Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "ZonesFtz214Permissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View Zones Ftz214 Records Permission")]
        ViewInboundRecord = 21200,

        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete Zones Ftz214 Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File Zones Ftz214 Records Permission")]
        FileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import Zones Ftz214 Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Rule Records
        /// </summary>
        [Description("View Zones Ftz214 Rules Permission")]
        ViewRules,

        /// <summary>
        /// Defines the permission to Edit Rule Records
        /// </summary>
        [Description("Edit Zones Ftz214 Rules Permission")]
        EditRules,

        /// <summary>
        /// Defines the permission to Delete Rule Records
        /// </summary>
        [Description("Delete Zones Ftz214 Rules Permission")]
        DeleteRules,

    }
}
