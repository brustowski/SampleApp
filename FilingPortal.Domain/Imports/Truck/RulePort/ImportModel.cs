using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Truck.RulePort
{
    /// <summary>
    /// Provides Excel file data mapping on Truck Import Port Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
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
