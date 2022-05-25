using Newtonsoft.Json;

namespace FilingPortal.Parts.Common.DataLayer.Entities
{
    /// <summary>
    /// Defines field model for update mapping values
    /// </summary>
    public class UpdateField
    {
        /// <summary>
        /// Gets or sets field identifier
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets value for this model
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets Record Id 
        /// </summary>
        [JsonProperty("record_id")]
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Parent Record Id
        /// </summary>
        [JsonProperty("parent_record_id")]
        public int ParentRecordId { get; set; }
    }
}