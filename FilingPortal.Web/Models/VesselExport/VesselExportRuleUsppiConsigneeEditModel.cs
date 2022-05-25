namespace FilingPortal.Web.Models.VesselExport
{
    /// <summary>
    /// Defines the Export Vessel Exporter-Consignee Rule Edit model
    /// </summary>
    public class VesselExportRuleUsppiConsigneeEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the USPPI identifier
        /// </summary>
        public string UsppiId { get; set; }
        /// <summary>
        /// Gets or sets the USPPI
        /// </summary>
        public string Usppi { get; set; }
        /// <summary>
        /// Gets or sets the Consignee identifier
        /// </summary>
        public string ConsigneeId { get; set; }
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
        /// Gets or sets the Consignee address identifier
        /// </summary>
        public string ConsigneeAddressId { get; set; }
        /// <summary>
        /// Get or sets the Consignee address
        /// </summary>
        public string ConsigneeAddress { get; set; }
    }
}