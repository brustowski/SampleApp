using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleProduct
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

            Map(p => p.ProductCode, "Product");
            Map(p => p.GrossWeightUnit, "Gross Weight Unit");
            Map(p => p.PackagesUnit, "Packages Unit");
            Map(p => p.InvoiceUQ, "Invoice UQ");
        }
    }
}
