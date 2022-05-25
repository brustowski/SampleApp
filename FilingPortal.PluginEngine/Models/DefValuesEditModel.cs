namespace FilingPortal.PluginEngine.Models
{
    /// <summary>
    /// Represents Rail Default Values Edit model
    /// </summary>
    public class DefValuesEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Display On UI
        /// </summary>
        public string DisplayOnUI { get; set; }
        /// <summary>
        /// Gets or sets Value Lable
        /// </summary>
        public string ValueLabel { get; set; }
        /// <summary>
        /// Gets or sets Value Description
        /// </summary>
        public string ValueDesc { get; set; }
        /// <summary>
        /// Gets or sets Default Value
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// Gets or sets UI Section
        /// </summary>
        public string UISection { get; set; }
        /// <summary>
        /// Gets or sets Manual
        /// </summary>
        public string Manual { get; set; }
        /// <summary>
        /// Gets or sets Has Default Value
        /// </summary>
        public bool HasDefaultValue { get; set; }
        /// <summary>
        /// Gets or sets Editable
        /// </summary>
        public bool Editable { get; set; }
        /// <summary>
        /// Gets or sets Mandatory
        /// </summary>
        public bool Mandatory { get; set; }
        /// <summary>
        /// Gets or sets the Column name, where value is stored
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets order and visibility on Single Filing form
        /// </summary>
        public byte? SingleFilingOrder { get; set; }
        /// <summary>
        /// Paired Field Table Name
        /// </summary>
        public string PairedFieldTable { get; set; }
        /// <summary>
        /// Paired Field Column name
        /// </summary>
        public string PairedFieldColumn { get; set; }
        /// <summary>
        /// Dropdown fields configuration
        /// </summary>
        public string HandbookName { get; set; }
        /// <summary>
        /// Gets or sets the Depends On field Id
        /// </summary>
        public int? DependsOnId { get; set; }
        /// <summary>
        /// Gets or sets the Overridden Type
        /// </summary>
        public string OverriddenType { get; set; }
        /// <summary>
        /// Gets or sets whether confirmation is required for this field
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
    }
}