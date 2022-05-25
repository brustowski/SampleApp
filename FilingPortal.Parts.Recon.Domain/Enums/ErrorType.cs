using System.ComponentModel;

namespace FilingPortal.Parts.Recon.Domain.Enums
{
    /// <summary>
    /// Enumerates possible error types on Cargowise screen
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// All mismatches
        /// </summary>
        [Description("All Mismatch")]
        All,
        /// <summary>
        /// Recon Value Flag
        /// </summary>
        [Description("Recon Value Flag Mismatch")]
        ReconValueFlag,
        /// <summary>
        /// Recon FTA Flag Mismatch
        /// </summary>
        [Description("Recon FTA Flag Mismatch")]
        ReconFtaFlag,
        /// <summary>
        /// Entry Value Mismatch
        /// </summary>
        [Description("Entry Value Mismatch")]
        EntryValue,
        /// <summary>
        /// Duty Mismatch
        /// </summary>
        [Description("Duty Mismatch")]
        Duty,
        /// <summary>
        /// MPF Mismatch
        /// </summary>
        [Description("MPF Mismatch")]
        Mpf,
        /// <summary>
        /// Payable MPF Mismatch
        /// </summary>
        [Description("Payable MPF Mismatch")]
        PayableMpf,
        /// <summary>
        /// Quantity Mismatch
        /// </summary>
        [Description("Quantity Mismatch")]
        Quantity,
        /// <summary>
        /// HTS Mismatch
        /// </summary>
        [Description("HTS Mismatch")]
        Hts,
        
    }
}
