using System;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Defines the Job Number Filter values
    /// </summary>
    [Flags]
    public enum JobNumberFilterValues
    {
        None = 0,
        ValueFlaggedNotFiled = 1,
        ValueFlaggedFiled = 2,
        FtaFlaggedNotFiled = 4,
        FtaFlaggedFiled = 8,
        NotFlagged = 16,
    }
}
