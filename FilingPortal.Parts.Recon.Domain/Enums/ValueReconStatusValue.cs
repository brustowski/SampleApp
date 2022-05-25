using System.ComponentModel;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Represents different Value Recon Statuses
    /// </summary>
    public enum ValueReconStatusValue
    {
        [Description("Open")]
        Open,
        [Description("Processed")]
        Processed,
        [Description("Error")]
        Error
    }
}
