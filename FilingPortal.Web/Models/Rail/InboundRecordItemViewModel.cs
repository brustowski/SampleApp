using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Class representing Inbound Record Item in View model
    /// </summary>
    public class InboundRecordItemViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets the identifier of Inbound Record Manifest record item
        /// </summary>
        public int ManifestRecordId { get; set; }
        /// <summary>
        /// Gets or sets the Importer Code of Inbound Record Item
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Issuer Code
        /// </summary>
        public string IssuerCode { get; set; }

        /// <summary>
        /// Gets or sets the Supplier Code of Inbound Record Item
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets the Bill Of Lading Number of Inbound Record Item
        /// </summary>
        public string BOLNumber { get; set; }
        /// <summary>
        /// Gets or sets the Container Number of Inbound Record Item
        /// </summary>
        public string ContainerNumber { get; set; }
        /// <summary>
        /// Gets or sets the Train Number of Inbound Record Item
        /// </summary>
        public string TrainNumber { get; set; }
        /// <summary>
        /// Gets or sets the Port of Unlading of Inbound Record Item
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// Gets or sets the HTS of Inbound Record Item
        /// </summary>
        public string HTS { get; set; }
        /// <summary>
        /// Gets or sets the Filing Status of Inbound Record Item
        /// </summary>
        public string HTSDescription { get; set; }
        /// <summary>
        /// Gets ot sets Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether Rail Inbound record marked as Deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}