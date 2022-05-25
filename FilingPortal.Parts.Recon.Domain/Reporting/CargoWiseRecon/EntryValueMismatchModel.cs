namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents the Entry Value Mismatch Report Model
    /// </summary>
    internal class EntryValueMismatchModel
    {
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ACE Entry Summary Number
        /// </summary>
        public string EntrySummaryNumber { get; set; }
        /// <summary>
        /// CW Job Number
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// CW Line number
        /// </summary>
        public string Line { get; set; }
        /// <summary>
        /// CW Recon Issue
        /// </summary>
        public string ReconIssue { get; set; }
        /// <summary>
        /// CW Line Entered Value
        /// </summary>
        public decimal? LineEnteredValue { get; set; }
        /// <summary>
        /// ACE Line Goods Value Amount
        /// </summary>
        public decimal LineGoodsValueAmount { get; set; }
        /// <summary>
        /// Mismatch massage
        /// </summary>
        public string Message { get; set; }
    }
}