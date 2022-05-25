using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Defines model for inbound records read model list representation
    /// </summary>
    public class InboundReadModel : InboundReadModelOld
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
        public DateTime? Eta { get; set; }
        /// <summary>
        /// Gets or sets Owners Reference
        /// </summary>
        public string OwnersReference { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public decimal? GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Direct ship date
        /// </summary>
        public DateTime? DirectShipDate { get; set; }
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public virtual string Consignee { get; set; }
        /// <summary>
        /// Gets or sets Product
        /// </summary>
        public virtual string Product { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public decimal? InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets the Line Price
        /// </summary>
        public decimal? LinePrice { get; set; }

        /// <summary>
        /// Gets or sets whether this record has Vendor rule
        /// </summary>
        public int HasVendorRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Port rule
        /// </summary>
        public int HasPortRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Product rule
        /// </summary>
        public int HasProductRule { get; set; }
    }
}
