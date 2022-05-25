using System;

namespace FilingPortal.Web.Models.Audit.Rail
{
    /// <summary>
    /// Rail Audit Daily Audit SPI Rules view model
    /// </summary>
    public class DailyAuditSpiRuleEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
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
        /// Gets or sets Customs Attribute 4
        /// </summary>
        public string CustomsAttrib4 { get; set; }
        /// <summary>
        /// SPI period date from
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// SPI period date to
        /// </summary>
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }

        /// <summary>
        /// Gets or sets Last Modified By
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets last modified date
        /// </summary>
        public string LastModifiedDate { get; set; }

    }
}