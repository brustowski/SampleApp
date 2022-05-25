using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.TruckExport.RuleConsignee
{
    /// <summary>
    /// Provides Excel file data mapping on Truck Export Consignee Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
    }
}
