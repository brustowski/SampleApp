using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RulePrice
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Price Rule");

            Map(p => p.ImporterCode, "Importer Code");
            Map(p => p.BatchCode, "Batch Code");
            Map(p => p.Facility, "Facility");
            Map(p => p.Pricing, "Pricing");
            Map(p => p.Freight, "Freight");
        }
    }
}
