using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Pipeline.RulePrice
{
    /// <summary>
    /// Represents Pipeline Inbound Import parsing data model
    /// </summary>
    public class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets Importer Code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Batch
        /// </summary>
        public string BatchCode { get; set; }
        /// <summary>
        /// Gets or sets Facility
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets or sets Pricing
        /// </summary>
        public string Pricing { get; set; }
        /// <summary>
        /// Gets or sets Freight
        /// </summary>
        public string Freight { get; set; }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Join("|", ImporterCode, BatchCode, Facility);
        }
    }
}
