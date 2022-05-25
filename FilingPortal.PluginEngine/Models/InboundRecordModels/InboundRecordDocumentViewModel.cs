using FilingPortal.Domain.Enums;
using Reinforced.Typings.Attributes;

namespace FilingPortal.PluginEngine.Models.InboundRecordModels
{
    /// <summary>
    /// Class describing model of Inbound Record Document used for view
    /// </summary>
    [TsInterface(Name = "DocumentViewModelServer", IncludeNamespace = false, AutoI = false, Order = 98)]
    public class InboundRecordDocumentViewModel
    {
        /// <summary>
        /// Gets or sets the document identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the document file name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the document description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the data
        /// </summary>
        public InboundRecordDocumentStatus Status { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this document is Manifest
        /// </summary>
        public bool IsManifest { get; set; }
    }
}