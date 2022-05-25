using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RuleImporter
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Importer Rule");

            Map(p => p.Importer, "Importer");
            Map(p => p.Supplier, "Supplier/Seller");
            Map(p => p.Manufacturer, "Manufacturer");
            Map(p => p.Consignee, "Consignee/Sold to Party/Ship to Party");
            Map(p => p.ReconIssue, "Recon. Issue");
            Map(p => p.FTARecon, "FTA Recon");
            Map(p => p.SPI, "SPI");
            Map(p => p.TransactionRelated, "Transaction Related");
            Map(p => p.CountryOfExport, "Country of Export");
            Map(p => p.Origin, "Country of Origin");
            Map(p => p.MID, "MID");
            Map(p => p.Seller, "Seller");
        }
    }
}
