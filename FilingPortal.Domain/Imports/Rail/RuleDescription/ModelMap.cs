using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Rail.RuleDescription
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
            Sheet("Product Description Rule");

            Map(p => p.Description1, "Description 1");
            Map(p => p.Importer, "Importer");
            Map(p => p.Supplier, "Supplier");
            Map(p => p.Port, "Port");
            Map(p => p.Destination, "Destination");
            Map(p => p.ProductID, "Product ID");
            Map(p => p.Attribute1, "Attribute 1");
            Map(p => p.Attribute2, "Attribute 2");
            Map(p => p.Tariff, "Tariff");
            Map(p => p.GoodsDescription, "Goods Description");
            Map(p => p.InvoiceUOM, "Invoice UOM");
            Map(p => p.TemplateHTSQuantity, "Template HTS Quantity");
            Map(p => p.TemplateInvoiceQuantity, "Template Invoice Quantity");
        }
    }
}
