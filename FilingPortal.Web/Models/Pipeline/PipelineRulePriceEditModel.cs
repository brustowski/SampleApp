namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents the Pipeline Rule Price Edit Model
    /// </summary>
    public class PipelineRulePriceEditModel
    {
        /// <summary>
        /// Gets or sets the rule identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Get or sets Client ID
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Client Code
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
