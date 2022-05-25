using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Truck
{
    /// <summary>
    /// Defines the Import Truck Rule View model
    /// </summary>
    public class TruckRuleImporterViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Arrival Port
        /// </summary>
        public string ArrivalPort { get; set; }

        /// <summary>
        /// Gets or sets the C/E
        /// </summary>
        public string CE { get; set; }

        /// <summary>
        /// Gets or sets the Charges
        /// </summary>
        public decimal? Charges { get; set; }

        /// <summary>
        /// Gets or sets the C/O
        /// </summary>
        public string CO { get; set; }
        
        /// <summary>
        /// Gets or sets the Custom Attribute 1
        /// </summary>
        public string CustomAttrib1 { get; set; }

        /// <summary>
        /// Gets or sets the Custom Attribute 2
        /// </summary>
        public string CustomAttrib2 { get; set; }

        /// <summary>
        /// Gets or sets the Custom Quantity
        /// </summary>
        public decimal? CustomQuantity { get; set; }

        /// <summary>
        /// Gets or sets the Custom UQ
        /// </summary>
        public string CustomUQ { get; set; }

        /// <summary>
        /// Gets or sets the CW IOR
        /// </summary>
        public string CWIOR { get; set; }

        /// <summary>
        /// Gets or sets the CW Supplier
        /// </summary>
        public string CWSupplier { get; set; }

        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the Destination State
        /// </summary>
        public string DestinationState { get; set; }

        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public decimal? GrossWeight { get; set; }

        /// <summary>
        /// Gets or sets the Gross Weight UQ
        /// </summary>
        public string GrossWeightUQ { get; set; }

        /// <summary>
        /// Gets or sets the Invoice QTY
        /// </summary>
        public decimal? InvoiceQTY { get; set; }

        /// <summary>
        /// Gets or sets the Invoice UQ
        /// </summary>
        public string InvoiceUQ { get; set; }

        /// <summary>
        /// Gets or sets the Line Price
        /// </summary>
        public decimal? LinePrice { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer MID
        /// </summary>
        public string ManufacturerMID { get; set; }

        /// <summary>
        /// Gets or sets the NAFTA Recon
        /// </summary>
        public string NAFTARecon { get; set; }

        /// <summary>
        /// Gets or sets the Product ID
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// Gets or sets the ReconIssue
        /// </summary>
        public string ReconIssue { get; set; }

        /// <summary>
        /// Gets or sets the SPI
        /// </summary>
        public string SPI { get; set; }

        /// <summary>
        /// Gets or sets the Supplier MID
        /// </summary>
        public string SupplierMID { get; set; }

        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }

        /// <summary>
        /// Gets or sets the Transactions Related
        /// </summary>
        public string TransactionsRelated { get; set; }
    }
}