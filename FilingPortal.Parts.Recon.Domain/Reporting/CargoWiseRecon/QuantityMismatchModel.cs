namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseRecon
{
    /// <summary>
    /// Represents the Quantity Mismatch Report Model
    /// </summary>
    internal class QuantityMismatchModel
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
        /// CW Customs Quantity 1
        /// </summary>
        public decimal? CustomsQty1 { get; set; }
        /// <summary>
        /// ACE Line Tariff Quantity
        /// </summary>
        public decimal LineTariffQuantity { get; set; }
        /// <summary>
        /// CW PSA Reason
        /// </summary>
        public string PsaReason { get; set; }
        /// <summary>
        /// CW PSA Reason 520d
        /// </summary>
        public string PsaReason520d { get; set; }
        /// <summary>
        /// Mismatch massage
        /// </summary>
        public string Message { get; set; }
    }
}