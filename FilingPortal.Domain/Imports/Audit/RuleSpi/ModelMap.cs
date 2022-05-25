using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Imports.Audit.Rule;

namespace FilingPortal.Domain.Imports.Audit.RuleSpi
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="DailyAuditRuleImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<DailyAuditSpiRuleImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditRuleImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Map(p => p.ImporterCode, "Importer");
            Map(p => p.SupplierCode, "Supplier");
            Map(p => p.GoodsDescription, "Goods Description");
            Map(p => p.DestinationState, "Destination State");
            Map(p => p.DateFrom, "Date From");
            Map(p => p.DateTo, "Date To");
            Map(p => p.Spi, "SPI");

            Sheet("Daily Audit SPI Rule");
        }
    }
}
