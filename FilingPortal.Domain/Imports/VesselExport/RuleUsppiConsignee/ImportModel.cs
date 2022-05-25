using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.VesselExport.RuleUsppiConsignee
{
    /// <summary>
    /// Provides Excel file data mapping on Vessel Export USPPI-Consignee Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the USPPI
        /// </summary>
        public string Usppi { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets the transactions related
        /// </summary>
        public string TransactionRelated { get; set; }
        /// <summary>
        /// Get or sets Ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
        /// <summary>
        /// Get or sets the Consignee address
        /// </summary>
        public string ConsigneeAddress { get; set; }
    }
}
