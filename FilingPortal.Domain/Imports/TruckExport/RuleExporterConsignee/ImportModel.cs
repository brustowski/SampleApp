using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.TruckExport.RuleExporterConsignee
{
    /// <summary>
    /// Provides Excel file data mapping on Truck Exporter-Consignee Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Exporter
        /// </summary>
        public string Exporter { get; set; }

        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the contact name
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the transactions related
        /// </summary>
        public string TranRelated { get; set; }
    }
}
