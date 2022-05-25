using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Isf.Domain.Entities
{
    /// <summary>
    /// Inbound record 
    /// </summary>
    public class InboundRecord : InboundEntity<FilingHeader>
    {
        /// <summary>
        /// Gets or sets Importer ID
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets buyer
        /// </summary>
        public Guid? BuyerId { get; set; }
        /// <summary>
        /// Gets or sets Buyer
        /// </summary>
        public virtual Client Buyer { get; set; }
        /// <summary>
        /// Gets or sets the Buyer address id
        /// </summary>
        public int? BuyerAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Buyer address
        /// </summary>
        public virtual AppAddress BuyerAppAddress { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public Guid? ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public virtual Client Consignee { get; set; }
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
        public Guid? SellerId { get; set; }
        /// <summary>
        /// Gets or sets Seller
        /// </summary>
        public virtual Client Seller { get; set; }
        /// <summary>
        /// Gets or sets the Seller address id
        /// </summary>
        public int? SellerAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Seller address
        /// </summary>
        public virtual AppAddress SellerAppAddress { get; set; }
        /// <summary>
        /// Gets or sets ship to id
        /// </summary>
        public Guid? ShipToId { get; set; }
        /// <summary>
        /// Gets or sets Ship To
        /// </summary>
        public virtual Client ShipTo { get; set; }
        /// <summary>
        /// Gets or sets the Ship To address id
        /// </summary>
        public int? ShipToAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Ship To address
        /// </summary>
        public virtual AppAddress ShipToAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Container Stuffing location Id
        /// </summary>
        public Guid? ContainerStuffingLocationId { get; set; }
        /// <summary>
        /// Gets or sets Container Stuffing Location
        /// </summary>
        public virtual Client ContainerStuffingLocation { get; set; }
        /// <summary>
        /// Gets or sets the Container Stuffing Location address id
        /// </summary>
        public int? ContainerStuffingLocationAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Container Stuffing Location address
        /// </summary>
        public virtual AppAddress ContainerStuffingLocationAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Consolidator Id
        /// </summary>
        public Guid? ConsolidatorId { get; set; }
        /// <summary>
        /// Gets or sets Consolidator/Forwarder
        /// </summary>
        public virtual Client Consolidator { get; set; }
        /// <summary>
        /// Gets or sets the Consolidator address id
        /// </summary>
        public int? ConsolidatorAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the Consolidator address
        /// </summary>
        public virtual AppAddress ConsolidatorAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets manufacturers for this record
        /// </summary>
        public virtual ICollection<InboundManufacturerRecord> Manufacturers { get; set; } = new List<InboundManufacturerRecord>();
        /// <summary>
        /// Gets or sets bills for this record
        /// </summary>
        public virtual ICollection<InboundBillRecord> Bills { get; set; } = new List<InboundBillRecord>();
        /// <summary>
        /// Gets or sets containers for this record
        /// </summary>
        public virtual ICollection<InboundContainerRecord> Containers { get; set; } = new List<InboundContainerRecord>();

    }
}
