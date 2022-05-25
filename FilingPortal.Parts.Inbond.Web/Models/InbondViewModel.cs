using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Inbond.Web.Models
{
    /// <summary>
    /// Defines the Inbond record item View Model
    /// </summary>
    public class InbondViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets the FIRMS code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets the Port of Arrival
        /// </summary>
        public string PortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets the Port Of Destination
        /// </summary>
        public string PortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets the Conveyance
        /// </summary>
        public string ExportConveyance { get; set; }
        /// <summary>
        /// Gets or sets the Entry Date
        /// </summary>
        public string EntryDate { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Quantity
        /// </summary>
        public string ManifestQty { get; set; }
        /// <summary>
        /// Gets or sets the Manifest Quantity Unit
        /// </summary>
        public string ManifestQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}