using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Common.Domain.Enums
{
    /// <summary>
    /// Defines the Filing Status
    /// </summary>
    [TsEnum(IncludeNamespace = false)]
    public enum FilingStatus : byte
    {
        /// <summary>
        /// Defines the Open status
        /// </summary>
        [Description("Open")]
        Open,

        /// <summary>
        /// Defines the In Progress status
        /// </summary>
        [Description("In Progress")]
        InProgress,

        /// <summary>
        /// Defines the Filed status
        /// </summary>
        [Description("Filed")]
        Filed,

        /// <summary>
        /// Defines the Error status
        /// </summary>
        [Description("Error")]
        Error,

        /// <summary>
        /// Defines the Updated status
        /// </summary>
        [Description("Updated")]
        Updated
    }
}
