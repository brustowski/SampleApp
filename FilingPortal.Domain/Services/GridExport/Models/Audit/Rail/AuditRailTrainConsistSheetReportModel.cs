namespace FilingPortal.Domain.Services.GridExport.Models.Audit.Rail
{
    /// <summary>
    /// Class describing Rail Train consist sheet model for reporting
    /// </summary>
    public class AuditRailTrainConsistSheetReportModel
    {
        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Entry Number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets the Bill number
        /// </summary>
        public string BillNumber { get; set; }
        /// <summary>
        /// Gets of sets the Audit status
        /// </summary>
        public string Status { get; set; }
    }
}
