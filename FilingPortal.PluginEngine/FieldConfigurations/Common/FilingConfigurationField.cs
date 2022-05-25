using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Represents filing field configuration
    /// </summary>
    [TsInterface(Name = "FilingConfigurationFieldServer", IncludeNamespace = false, AutoI = false, Order = 200)]
    public class FilingConfigurationField
    {
        /// <summary>
        /// Gets or sets the filing configuration field identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the filing header identifier
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the record identifier
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// Gets or sets the parent record id
        /// </summary>
        public int ParentRecordId { get; set; }
        
        /// <summary>
        /// Gets or sets the section name
        /// </summary>
        public string SectionName { get; set; }
        
        /// <summary>
        /// Gets or sets the section title
        /// </summary>
        public string SectionTitle { get; set; }

        /// <summary>
        /// Gets or sets field order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether this field is visible on 7501 form
        /// </summary>
        public bool IsVisibleOn7501 { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether this field is visible on rule driven data form
        /// </summary>
        public bool IsVisibleOnRuleDrivenData { get; set; }
        
        /// <summary>
        /// Gets or sets the field title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default field value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of value
        /// </summary>
        public string Type { get; set; }

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
        /// Underlying field
        /// </summary>
        public BaseInboundRecordField Field { get; set; }
    }
}