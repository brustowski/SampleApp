using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RuleBatchCode
{
    /// <summary>
    /// Provides Excel file data mapping on Pipeline Import Batch Code Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets the Batch Code
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public string Product { get; set; }
    }
}
