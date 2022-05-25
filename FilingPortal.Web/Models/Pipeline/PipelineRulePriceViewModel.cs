using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Price View Model
    /// </summary>
    public class PipelineRulePriceViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Get or sets Importer ID
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer Code
        /// </summary>
        public string Importer { get; set; }

        /// <summary>
        /// Gets or sets the Crude Type ID
        /// </summary>
        public int? CrudeTypeId { get; set; }
        /// <summary>
        /// Gets or sets the Crude Type
        /// </summary>
        public string CrudeType { get; set; }
        /// <summary>
        /// Gets or sets the Facility ID
        /// </summary>
        public int? FacilityId { get; set; }
        /// <summary>
        /// Gets or sets the Facility
        /// </summary>
        public string Facility { get; set; }

        /// <summary>
        /// Gets or sets the pricing
        /// </summary>
        public decimal Pricing { get; set; }

        /// <summary>
        /// Gets of sets the Freight
        /// </summary>
        public decimal Freight { get; set; }
    }
}
