using System;
using System.Collections.Generic;
using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.PluginEngine.Models.Fields;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Parts.Isf.Web.Models.Inbound
{
    /// <summary>
    /// Defines the inbound record View Model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true, Name = "IsfInboundEditModel")]
    public class InboundRecordEditModel
    {
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Importer ID
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets buyer
        /// </summary>
        public string BuyerId { get; set; }
        /// <summary>
        /// Gets or sets the Buyer address id
        /// </summary>
        public AddressFieldEditModel BuyerAppAddress { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public string ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets master bill of lading SCAC Code
        /// </summary>
        public string MblScacCode { get; set; }
        /// <summary>
        /// Gets or sets Etd
        /// </summary>
        public DateTime? Eta { get; set; }
        /// <summary>
        /// Gets or sets Etd
        /// </summary>
        public DateTime? Etd { get; set; }
        /// <summary>
        /// Gets or sets seller
        /// </summary>
        public string SellerId { get; set; }
        /// <summary>
        /// Gets or sets the Seller address id
        /// </summary>
        public AddressFieldEditModel SellerAppAddress { get; set; }
        /// <summary>
        /// Gets or sets ship to id
        /// </summary>
        public string ShipToId { get; set; }
        /// <summary>
        /// Gets or sets the Ship To address id
        /// </summary>
        public AddressFieldEditModel ShipToAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Container Stuffing location Id
        /// </summary>
        public string ContainerStuffingLocationId { get; set; }
        /// <summary>
        /// Gets or sets the Container Stuffing Location address id
        /// </summary>
        public AddressFieldEditModel ContainerStuffingLocationAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Consolidator Id
        /// </summary>
        public string ConsolidatorId { get; set; }
        /// <summary>
        /// Gets or sets the Consolidator address id
        /// </summary>
        public AddressFieldEditModel ConsolidatorAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets manufacturers
        /// </summary>
        public IEnumerable<InboundManufacturerRecordEditModel> Manufacturers { get; set; }
        /// <summary>
        /// Gets or sets bills collection
        /// </summary>
        public IEnumerable<BillsRecordEditModel> Bills { get; set; }
        /// <summary>
        /// Gets or sets containers collection
        /// </summary>
        public IEnumerable<ContainersRecordEditModel> Containers { get; set; }
    }
}