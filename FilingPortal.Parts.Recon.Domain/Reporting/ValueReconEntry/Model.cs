namespace FilingPortal.Parts.Recon.Domain.Reporting.ValueReconEntry
{
    /// <summary>
    /// Represents the Value Recon Entry Report Model
    /// </summary>
    internal class Model
    {
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Filer Entry Number
        /// </summary>
        public string FilerEntryNo { get; set; }
        /// <summary>
        /// Gets or sets the Duty/Fees
        /// </summary>
        public string DutyFees { get; set; }
    }
}