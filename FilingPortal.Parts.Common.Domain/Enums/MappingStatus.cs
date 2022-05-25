using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Common.Domain.Enums
{
    /// <summary>
    /// Defines the Mapping Status
    /// </summary>
    [TsEnum(IncludeNamespace = false)]
    public enum MappingStatus : byte
    {
        /// <summary>
        /// Defines the Open status
        /// </summary>
        [Description("Open")]
        Open,

        /// <summary>
        /// Defines the In Review status
        /// </summary>
        [Description("In Review")]
        InReview,

        /// <summary>
        /// Defines the In Progress status
        /// </summary>
        [Description("In Progress")]
        InProgress,

        /// <summary>
        /// Defines the Mapped status
        /// </summary>
        [Description("Mapped")]
        Mapped,

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
