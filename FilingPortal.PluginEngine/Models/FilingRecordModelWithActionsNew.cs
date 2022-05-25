namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents Filing Record View
    /// </summary>
    public abstract class FilingRecordModelWithActionsNew : BaseFilingRecordModelWithActions
    {
        /// <summary>
        /// Gets or sets the Job Status
        /// </summary>
        public int JobStatus { get; set; }

        /// <summary>
        /// Gets or sets the Job Status Title
        /// </summary>
        public string JobStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets Job Status code
        /// </summary>
        public string JobStatusCode { get; set; }
        /// <summary>
        /// Gets or sets value that indicates that record is valid or not
        /// </summary>
        public bool ValidationPassed { get; set; }
        /// <summary>
        /// Gets or sets the validation results value
        /// </summary>
        public string ValidationResult { get; set; }
        /// <summary>
        /// Gets or sets the Entry Status
        /// </summary>
        public string EntryStatus { get; set; }
        /// <summary>
        /// Gets or sets the Entry Status Description
        /// </summary>
        public string EntryStatusDescription { get; set; }
        /// <summary>
        /// Gets or sets whether this record was filed automatically
        /// </summary>
        public bool IsAuto { get; set; }
    }
}