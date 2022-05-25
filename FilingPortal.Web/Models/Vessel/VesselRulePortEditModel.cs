namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Rule Port Edit model
    /// </summary>
    public class VesselRulePortEditModel
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
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }

        /// <summary>
        /// Gets or sets FIRMs Code id
        /// </summary>
        public string FirmsCodeId { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }

        /// <summary>
        /// Gets or sets the HMF
        /// </summary>
        public string HMF { get; set; }
        /// <summary>
        /// Gets or sets the Destination Code
        /// </summary>
        public string DestinationCode { get; set; }
    }
}