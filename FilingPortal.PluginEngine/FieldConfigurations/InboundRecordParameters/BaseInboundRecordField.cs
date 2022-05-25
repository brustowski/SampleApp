using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Class representing Inbound Record field description
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false)]
    public abstract class BaseInboundRecordField
    {
        /// <summary>
        /// Gets or sets the field title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the type of value
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Gets or sets whether field should be shown on applying Fields to Review filter
        /// </summary>
        public bool MarkedForFiltering { get; set; } = false;
    }
}