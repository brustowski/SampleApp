using System.Collections.Generic;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.FieldConfigurations.Common
{
    /// <summary>
    /// Represents filing configuration
    /// </summary>
    [TsInterface(Name = "FilingConfigurationServer", IncludeNamespace = false, AutoI = false, Order = 205)]
    public class FilingConfiguration
    {
        /// <summary>
        /// Gets or sets the filing header identifier
        /// </summary>
        public int FilingHeaderId { get; set; }
        /// <summary>
        /// Gets or sets the list of fields
        /// </summary>
        public List<FilingConfigurationField> Fields { get; set; }
        /// <summary>
        /// Gets or sets the list of sections
        /// </summary>
        public List<FilingConfigurationSection> Sections { get; set; }
        /// <summary>
        /// Gets or sets the list of documents
        /// </summary>
        public List<InboundRecordDocumentViewModel> Documents { get; set; }
    }
}