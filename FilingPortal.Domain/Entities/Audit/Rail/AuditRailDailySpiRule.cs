using System;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Audit.Rail
{
    /// <summary>
    /// Represents model for rail daily audit SPI rule
    /// </summary>
    public class AuditRailDailySpiRule : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Supplier code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets goods description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets destination state
        /// </summary>
        public string DestinationState { get; set; }
        /// <summary>
        /// SPI period date from
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// SPI period date to
        /// </summary>
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 4
        /// </summary>
        public string CustomsAttrib4 { get; set; }
        /// <summary>
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }
        /// <summary>
        /// Gets or sets the author
        /// </summary>
        public virtual AppUsersModel Author { get; set; }
    }
}
