using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Rail.RuleDescription
{
    /// <summary>
    /// Provides Excel file data mapping on Rail Import Description Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Description1
        /// </summary>
        public string Description1 { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets the Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets the Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets Product ID
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 1
        /// </summary>
        public string Attribute1 { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 2
        /// </summary>
        public string Attribute2 { get; set; }
        /// <summary>
        /// Gets or sets Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets Invoice Units Of Measure
        /// </summary>
        public string InvoiceUOM { get; set; }
        /// <summary>
        /// Gets or sets Template HTS Quantity
        /// </summary>
        public string TemplateHTSQuantity { get; set; }
        /// <summary>
        /// Gets or sets Template Invoice Quantity
        /// </summary>
        public string TemplateInvoiceQuantity { get; set; }
    }
}
