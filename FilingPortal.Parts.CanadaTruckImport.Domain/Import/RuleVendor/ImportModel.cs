using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleVendor
{
    /// <summary>
    /// Provides Excel file data mapping on Vendor Rule Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
    {
        /// <summary>
        /// Gets or sets vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Gets or sets Vendor
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Exporter
        /// </summary>
        public string Exporter { get; set; }
        /// <summary>
        /// Gets or sets the Export State
        /// </summary>
        public string ExportState { get; set; }
        /// <summary>
        /// Gets or sets Direct Ship place
        /// </summary>
        public string DirectShipPlace { get; set; }
        /// <summary>
        /// Gets or sets No. Packages
        /// </summary>
        public string NoPackages { get; set; }
        /// <summary>
        /// Gets or sets the Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets ORG state
        /// </summary>
        public string OrgState { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return Vendor;
        }
    }
}
