using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RuleImporter
{
    /// <summary>
    /// Provides Excel file data mapping on Pipeline Importer Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// Gets or sets the Country of Export
        /// </summary>
        public string CountryOfExport { get; set; }

        /// <summary>
        /// Gets or sets the Freight
        /// </summary>
        public string Freight { get; set; }

        /// <summary>
        /// Gets or sets the FTA Recon
        /// </summary>
        public string FTARecon { get; set; }

        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer Address
        /// </summary>
        public string ManufacturerAddress { get; set; }

        /// <summary>
        /// Gets or sets the MID
        /// </summary>
        public string MID { get; set; }

        /// <summary>
        /// Gets or sets the Origin
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the Recon Issue
        /// </summary>
        public string ReconIssue { get; set; }

        /// <summary>
        /// Gets or sets the Seller
        /// </summary>
        public string Seller { get; set; }

        /// <summary>
        /// Gets or sets the SPI
        /// </summary>
        public string SPI { get; set; }

        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// Gets or sets the Transaction Related
        /// </summary>
        public string TransactionRelated { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }
    }
}
