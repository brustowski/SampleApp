using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets Vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Pars Number
        /// </summary>
        public string ParsNumber { get; set; }
        /// <summary>
        /// Gets or sets ETA
        /// </summary>
        public string Eta { get; set; }
        /// <summary>
        /// Gets or sets Owners Reference
        /// </summary>
        public string OwnersReference { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public string GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Direct ship date
        /// </summary>
        public string DirectShipDate { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets Product
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public string InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets the Line Price
        /// </summary>
        public string LinePrice { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}