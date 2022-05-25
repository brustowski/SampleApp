namespace FilingPortal.Web.Models.Truck
{
    /// <summary>
    /// Defines the Truck Rule Port Edit model
    /// </summary>
    public class TruckRulePortEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Arrival Port
        /// </summary>
        public string ArrivalPort { get; set; }

        /// <summary>
        /// Gets or sets the FIRMsCode
        /// </summary>
        public string FIRMsCode { get; set; }
    }
}