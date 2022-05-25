namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents Filing Record View
    /// </summary>
    public abstract class FilingRecordModelWithActionsOld : BaseFilingRecordModelWithActions
    {
        /// <summary>
        /// Gets or sets the Mapping Status
        /// </summary>
        public int MappingStatus { get; set; }

        /// <summary>
        /// Gets or sets the Mapping Status Title
        /// </summary>
        public string MappingStatusTitle { get; set; }

        /// <summary>
        /// Gets or sets the Filing Status
        /// </summary>
        public int FilingStatus { get; set; }

        /// <summary>
        /// Gets or sets the Filing Status Title
        /// </summary>
        public string FilingStatusTitle { get; set; }
        
        /// <summary>
        /// Gets or sets the Entry Status
        /// </summary>
        public string EntryStatus { get; set; }

        /// <summary>
        /// Gets or sets the Entry Status Description
        /// </summary>
        public string EntryStatusDescription { get; set; }
        /// <summary>
        /// Gets or sets whether model has all required rules
        /// </summary>
        public bool HasAllRequiredRules { get; set; }
    }
}