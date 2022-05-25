using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Domain.Enums
{
    /// <summary>
    /// Defines the plugin permissions
    /// </summary>
    [TsEnum(IncludeNamespace = false, Name = "IsfPermissions")]
    public enum Permissions
    {
        /// <summary>
        /// Defines the permission to View Records
        /// </summary>
        [Description("View ISF Records Permission")]
        ViewInboundRecord = 22001,
        /// <summary>
        /// Defines the permission to Delete Records
        /// </summary>
        [Description("Delete ISF Records Permission")]
        DeleteInboundRecord,

        /// <summary>
        /// Defines the permission to File Records
        /// </summary>
        [Description("File ISF Records Permission")]
        FileInboundRecord,
        /// <summary>
        /// Defines the permission to Add Records
        /// </summary>
        [Description("Add ISF Records Permission")]
        AddInboundRecord
    }
}
