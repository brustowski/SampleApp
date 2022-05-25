using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Rail.RulePort
{
    /// <summary>
    /// Provides Excel file data mapping on Rail Import Port Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Port
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// Gets or sets Origin
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// Gets or sets Destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets FIRMs Code
        /// </summary>
        public string FIRMsCode { get; set; }
        /// <summary>
        /// Gets or sets Export
        /// </summary>
        public string Export { get; set; }
    }
}
