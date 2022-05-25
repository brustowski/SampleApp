using System.ComponentModel;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Represents different FTA Recon Statuses
    /// </summary>
    public enum FtaReconStatusValue
    {
        [Description("Open")]
        Open,
        [Description("In Progress")]
        InProgress,
        [Description("Updated")]
        Updated,
        [Description("Error")]
        Error
    }
}
