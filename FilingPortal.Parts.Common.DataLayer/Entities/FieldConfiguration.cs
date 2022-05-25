using Newtonsoft.Json;

namespace FilingPortal.Parts.Common.DataLayer.Entities
{
    /// <summary>
    /// Defines base DefValue Manual Read model entity
    /// </summary>
    public class FieldConfiguration
    {
        /// <summary>
        /// Gets or sets field identifier
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets whether this model should be displayed on UI (0-false, 1..n-true, serial number)
        /// </summary>
        [JsonProperty("display_on_ui")]
        public byte DisplayOnUi { get; set; }
        /// <summary>
        /// Gets or sets the Filing Header id
        /// </summary>
        [JsonProperty("filing_header_id")]
        public int FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets Column Name
        /// </summary>
        [JsonProperty("column_name")]
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets value for this model
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets field UI label
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }
        /// <summary>
        /// Gets or sets whether this model is mandatory
        /// </summary>
        [JsonProperty("mandatory")]
        public bool Mandatory { get; set; }
        /// <summary>
        /// Gets or sets field database type
        /// </summary>
        [JsonProperty("value_type")]
        public string ValueType { get; set; }
        /// <summary>
        /// Gets or sets field max length
        /// </summary>
        [JsonProperty("value_max_length")]
        public int? ValueMaxLength { get; set; }
        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        [JsonProperty("table_name")]
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets whether this model is editable
        /// </summary>
        [JsonProperty("editable")]
        public bool Editable { get; set; }
        /// <summary>
        /// Gets or sets whether this model has default value
        /// </summary>
        [JsonProperty("has_default_value")]
        public bool HasDefaultValue { get; set; }
        /// <summary>
        /// Gets or sets whether this model should be displayed on 7501 form (0-false, 1..n-true, serial number)
        /// </summary>
        [JsonProperty("manual")]
        public byte Manual { get; set; }
        /// <summary>
        /// Gets or sets the section name, where field is located
        /// </summary>
        [JsonProperty("section_title")]
        public string SectionTitle { get; set; }
        /// <summary>
        /// Paired Field Table Name
        /// </summary>
        [JsonProperty("paired_field_table")]
        public string PairedFieldTable { get; set; }
        /// <summary>
        /// Paired Field Column name
        /// </summary>
        [JsonProperty("paired_field_column")]
        public string PairedFieldColumn { get; set; }
        /// <summary>
        /// Dropdown fields configuration
        /// </summary>
        [JsonProperty("handbook_name")]
        public string HandbookName { get; set; }
        /// <summary>
        /// Gets or sets Parent Record Id
        /// </summary>
        [JsonProperty("parent_record_id")]
        public int ParentRecordId { get; set; }
        /// <summary>
        /// Gets or sets Section Name
        /// </summary>
        [JsonProperty("section_name")]
        public string SectionName { get; set; }
        /// <summary>
        /// Gets or sets Record Id 
        /// </summary>
        [JsonProperty("record_id")]
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Depends On Column
        /// </summary>
        [JsonProperty("depends_on")]
        public string DependsOn { get; set; }
        /// <summary>
        /// Gets or sets whether confirmation is needed
        /// </summary>
        [JsonProperty("confirmation_needed")]
        public string ConfirmationNeeded { get; set; }
    }
}
