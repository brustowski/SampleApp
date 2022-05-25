using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Enums
{
    /// <summary>
    /// Defines the Canada Truck Import Permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "CanadaImpTruckPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View Canada Truck Import Records Permission")]
        ViewInboundRecord = 20001,

        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete Canada Truck Import Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File Canada Truck Import Records Permission")]
        FileInboundRecord,

        /// <summary>
        /// Defines the permission to Import Records
        /// </summary>
        [Description("Import Canada Truck Import Records Permission")]
        ImportInboundRecord,

        /// <summary>
        /// Defines the permission to View Rule Records
        /// </summary>
        [Description("View Canada Truck Rules Permission")]
        ViewRules,

        /// <summary>
        /// Defines the permission to Edit Rule Records
        /// </summary>
        [Description("Edit Canada Truck Rules Permission")]
        EditRules,

        /// <summary>
        /// Defines the permission to Delete Rule Records
        /// </summary>
        [Description("Delete Canada Truck Rules Permission")]
        DeleteRules,
    }
}
