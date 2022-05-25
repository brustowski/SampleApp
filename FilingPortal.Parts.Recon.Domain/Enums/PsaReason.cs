using System;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Defines the PSA Reason 
    /// </summary>
    [Flags]
    public enum PsaReason
    {
        None = 0,
        Flagged = 1,
        NotFlagged = 2,
        Filed = 4,
        NotFiled = 8
    }
}
