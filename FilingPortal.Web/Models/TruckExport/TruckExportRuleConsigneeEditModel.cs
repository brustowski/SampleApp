namespace FilingPortal.Web.Models.TruckExport
{
    /// <summary>
    /// Defines the Export Truck Consignee Rule Edit model
    /// </summary>
    public class TruckExportRuleConsigneeEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
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