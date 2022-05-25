using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Rule Importer View model
    /// </summary>
    public class VesselRuleImporterViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the CargoWise Importer
        /// </summary>
        public string CWImporter { get; set; }
    }
}