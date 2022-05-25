namespace FilingPortal.Parts.Common.Domain.DTOs
{
    /// <summary>
    /// Represents the Import form filing parameter
    /// </summary>
    public class ImportFormParameter
    {
        /// <summary>
        /// Gets or sets section
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// Gets or sets column
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// Gets or sets value for this model
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets Parent Record Id
        /// </summary>
        public int ParentRecordId { get; set; }
        /// <summary>
        /// Gets or sets Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets row number 
        /// </summary>
        public int RowNumber { get; set; }
    }
}
