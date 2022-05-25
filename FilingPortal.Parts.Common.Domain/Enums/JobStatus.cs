using System.ComponentModel;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Common.Domain.Enums
{
    /// <summary>
    /// Enum represents different Job Statuses
    /// </summary>
    [TsEnum(IncludeNamespace = false)]
    public enum JobStatus: byte
    {
        [Description("Open")]
        Open,
        [Description("In Review")]
        InReview,
        [Description("In Progress")]
        InProgress,
        [Description("Created")]
        Created,
        [Description("Mapping Error")]
        MappingError,
        [Description("Creating Error")]
        CreatingError,
        [Description("Waiting Update")]
        WaitingUpdate,
        [Description("Updated")]
        Updated,
        [Description("Updating Error")]
        UpdatingError
    }
}
