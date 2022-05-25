using System;

namespace FilingPortal.Domain.Services.GridExport.Models.Audit.Rail
{
    /// <summary>
    /// Class describing Rail Train daily audit SPI rules model for reporting
    /// </summary>
    public class AuditRailDailyAuditSpiRulesReportModel
    {
        /// <summary>
        /// Record Id
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
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}
