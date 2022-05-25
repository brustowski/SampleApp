using System;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Parts.Isf.Domain.Entities
{
    /// <summary>
    /// Entity, representing Manufacturer Record
    /// </summary>
    public class InboundManufacturerRecord : AuditableEntity
    {
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Gets or sets Inbound record
        /// </summary>
        public virtual InboundRecord Inbound { get; set; }
        /// <summary>
        /// Gets or sets Manufacturer is
        /// </summary>
        public Guid? ManufacturerId { get; set; }
        /// <summary>
        /// Gets or sets Manufacturer
        /// </summary>
        public virtual Client Manufacturer { get; set; }
        /// <summary>
        /// Gets or sets the address id
        /// </summary>
        public int? ManufacturerAppAddressId { get; set; }
        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public virtual AppAddress ManufacturerAppAddress { get; set; }
        /// <summary>
        /// Gets or sets Item/Part number
        /// </summary>
        public string ItemNumber { get; set; }
        /// <summary>
        /// Gets or sets Country Of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets HTS numbers
        /// </summary>
        public string HtsNumbers { get; set; }
    }
}
