using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Isf.Domain.Entities
{
    /// <summary>
    /// Defines model for inbound records read model list representation
    /// </summary>
    public class InboundReadModel : InboundReadModelOld
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
        public DateTime? Eta { get; set; }
        /// <summary>
        /// Gets or sets Etd
        /// </summary>
        public DateTime? Etd { get; set; }
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
        /// Determines whether initial record data can be edited
        /// </summary>
        public virtual bool CanEditInitialRecord() => !FilingHeaderId.HasValue || MappingStatus == Common.Domain.Enums.MappingStatus.Open && FilingStatus == Common.Domain.Enums.FilingStatus.Open;
    }
}
