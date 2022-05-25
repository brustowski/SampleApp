namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents Filing Record View
    /// </summary>
    public abstract class BaseFilingRecordModelWithActions : ViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Creation Date of record
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the Filing Number of record
        /// </summary>
        public string FilingNumber { get; set; }

        /// <summary>
        /// Gets or sets the link to the job in the CargoWise system
        /// </summary>
        public string JobLink { get; set; }
        
        /// <summary>
        /// Gets or sets the Filing Header identifier
        /// </summary>
        public string FilingHeaderId { get; set; }

    }
}