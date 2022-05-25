using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Vessel.RulePort
{
    /// <summary>
    /// Provides Excel file data mapping on Vessel Import Port Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }

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
