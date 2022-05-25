using System;
using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Audit.RuleSpi
{
    /// <summary>
    /// Provides Excel file data mapping on Daily Audit SPI Rule Import model
    /// </summary>
    internal class DailyAuditSpiRuleImportModel : ParsingDataModel
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
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }
    }
}
