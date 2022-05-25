using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Truck.RuleImporter
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

            Map(p => p.CWIOR, "Importer Code");
            Map(p => p.CWSupplier, "Supplier Code");
            Map(p => p.ConsigneeCode, "Consignee Code");
            Map(p => p.SupplierMID, "Supplier MID");
            Map(p => p.ManufacturerMID, "Manufacturer MID");
            Map(p => p.ReconIssue, "Recon Issue");
            Map(p => p.NAFTARecon, "NAFTA Recon");
            Map(p => p.TransactionsRelated, "Transactions Related");
            Map(p => p.EntryPort, "Entry Port");
            Map(p => p.ArrivalPort, "Arrival Port");
            Map(p => p.DestinationState, "Destination State");
            Map(p => p.CE, "Country of Export");
            Map(p => p.CO, "Country of Origin");
            Map(p => p.Tariff, "Tariff");
            Map(p => p.SPI, "SPI");
            Map(p => p.CustomQuantity, "Custom Quantity");
            Map(p => p.CustomUQ, "Custom UQ");
            Map(p => p.InvoiceQTY, "Invoice QTY");
            Map(p => p.InvoiceUQ, "Invoice UQ");
            Map(p => p.LinePrice, "Line Price");
            Map(p => p.GrossWeight, "Gross Weight");
            Map(p => p.GrossWeightUQ, "Gross Weight UQ");
            Map(p => p.GoodsDescription, "Goods Description");
            Map(p => p.CustomAttrib1, "Custom Attrib 1");
            Map(p => p.CustomAttrib2, "Custom Attrib 2");
            Map(p => p.ProductID, "Product ID");
            Map(p => p.Charges, "Charges");
        }
    }
}
