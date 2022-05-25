using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Isf.Web.Models.Inbound
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    public class InboundRecordViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
    {
        /// <summary>
        /// Gets or sets importer
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Buyer
        /// </summary>
        public string BuyerCode { get; set; }
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets master bill of lading SCAC Code
        /// </summary>
        public string MblScacCode { get; set; }
        /// <summary>
        /// Gets or sets Etd
        /// </summary>
        public string Eta { get; set; }
        /// <summary>
        /// Gets or sets Etd
        /// </summary>
        public string Etd { get; set; }
        /// <summary>
        /// Gets or sets Seller Code
        /// </summary>
        public string SellerCode { get; set; }
        /// <summary>
        /// Gets or sets Ship To client code
        /// </summary>
        public string ShipToCode { get; set; }
        /// <summary>
        /// Gets or sets Container Stuffing location code
        /// </summary>
        public string ContainerStuffingLocationCode { get; set; }
        /// <summary>
        /// Gets or sets Consolidator Code
        /// </summary>
        public string ConsolidatorCode { get; set; }
        /// <summary>
        /// Gets or sets Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets Record Creator
        /// </summary>
        public virtual string CreatedUser { get; set; }
        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}