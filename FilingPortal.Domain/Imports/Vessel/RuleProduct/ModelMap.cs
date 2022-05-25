using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Vessel.RuleProduct
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
            Sheet("Product Rule");

            Map(p => p.Tariff, "Tariff");
            Map(p => p.GoodsDescription, "Goods Description");
            Map(p => p.CustomsAttribute1, "Customs Attribute 1");
            Map(p => p.CustomsAttribute2, "Customs Attribute 2");
            Map(p => p.InvoiceUQ, "Invoice UQ");
            Map(p => p.TSCARequirement, "TSCA Requirement");
        }
    }
}
