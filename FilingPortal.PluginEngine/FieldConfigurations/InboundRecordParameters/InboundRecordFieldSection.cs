using System.Collections.Generic;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Class describing section with Inbound Record fields
    /// </summary>
    public class InboundRecordFieldSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordFieldSection"/> class
        /// </summary>
        public InboundRecordFieldSection()
        {
            Fields = new List<BaseInboundRecordField>();
        }

        /// <summary>
        /// Gets or sets the name of the section
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public List<BaseInboundRecordField> Fields { get; set; }
    }
}