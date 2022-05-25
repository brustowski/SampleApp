using Newtonsoft.Json;

namespace FilingPortal.Parts.Common.DataLayer.Entities
{
    /// <summary>
    /// Defines field model for import mapping values
    /// </summary>
    public class ImportField
    {
        /// <summary>
        /// Gets or sets field section
        /// </summary>
        [JsonProperty("section")]
        public string Section { get; set; }
        /// <summary>
        /// Gets or sets field column
        /// </summary>
        [JsonProperty("column")]
        public string Column { get; set; }
        /// <summary>
        /// Gets or sets value for this model
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets row number 
        /// </summary>
        [JsonProperty("row_number")]
        public int RowNumber { get; set; }
        /// <summary>
        /// Gets or sets Parent Record Id
        /// </summary>
        [JsonProperty("parent_record_id")]
        public int ParentRecordId { get; set; }
        /// <summary>
        /// Gets or sets Filing Header Id
        /// </summary>
        [JsonProperty("filing_header_id")]
        public int FilingHeaderId { get; set; }
    }
}
