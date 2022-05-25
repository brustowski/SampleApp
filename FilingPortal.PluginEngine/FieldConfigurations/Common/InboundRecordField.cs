using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Class representing Inbound Record field description
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public class InboundRecordField: BaseInboundRecordField
    {
        /// <summary>
        /// Gets or sets the identifier of Inbound Record field description
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the field record identifier 
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets parent record id
        /// </summary>
        public int ParentRecordId { get; set; }

        /// <summary>
        /// Gets or sets the filing header identifier
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the default field value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum length of field input
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is mandatory
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this field is disabled for editing
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets value prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the field dependency from another field by name
        /// </summary>
        public string DependOn { get; set; }
        /// <summary>
        /// Gets or sets whether this field requires confirmation
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
        /// <summary>
        /// Gets or sets UI class for records
        /// </summary>
        public string Class { get; set; }
    }
}