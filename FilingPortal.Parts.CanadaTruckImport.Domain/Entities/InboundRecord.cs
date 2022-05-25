using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Inbound record 
    /// </summary>
    public class InboundRecord : InboundEntity<FilingHeader>
    {
        /// <summary>
        /// Gets or sets vendor id
        /// </summary>
        public Guid VendorId { get; set; }
        /// <summary>
        /// Gets or sets vendor
        /// </summary>
        public virtual Client Vendor { get; set; }
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
        public DateTime Eta { get; set; }
        /// <summary>
        /// Gets or sets Owners Reference
        /// </summary>
        public string OwnersReference { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Direct ship date
        /// </summary>
        public DateTime DirectShipDate { get; set; }
        /// <summary>
        /// Gets or sets consignee id
        /// </summary>
        public Guid ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets consignee
        /// </summary>
        public virtual Client Consignee { get; set; }
        /// <summary>
        /// Gets or sets product code id
        /// </summary>
        public Guid ProductCodeId { get; set; }
        /// <summary>
        /// Gets or sets Product
        /// </summary>
        public virtual ProductCode ProductCode { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public decimal InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets the Line Price
        /// </summary>
        public decimal LinePrice { get; set; }
    }
}
