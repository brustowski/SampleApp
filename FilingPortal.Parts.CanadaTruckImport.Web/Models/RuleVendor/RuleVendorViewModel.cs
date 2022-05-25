using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleVendor
{
    /// <summary>
    /// Describes Vendor Rule View Model
    /// </summary>
    public class RuleVendorViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets vendor id
        /// </summary>
        public string VendorId { get; set; }
        /// <summary>
        /// Gets or sets vendor
        /// </summary>
        public string Vendor { get; set; }
        /// <summary>
        /// Gets or sets importer id
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets exporter id
        /// </summary>
        public string ExporterId { get; set; }
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
    }
}