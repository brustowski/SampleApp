using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Entities.Pipeline
{
    using Framework.Domain;
    using System;

    /// <summary>
    /// Provides the Pipeline Rule for prices
    /// </summary>
    public class PipelineRulePrice : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Get or sets Client ID
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Client Code
        /// </summary>
        public virtual Client Importer { get; set; }

        /// <summary>
        /// Gets or sets the Crude Type ID
        /// </summary>
        public int? CrudeTypeId { get; set; }
        /// <summary>
        /// Gets or sets the Crude Type
        /// </summary>
        public virtual PipelineRuleBatchCode CrudeType { get; set; }
        /// <summary>
        /// Gets or sets Facility ID
        /// </summary>
        public int? FacilityId { get; set; }
        /// <summary>
        /// Gets or sets facility rule
        /// </summary>
        public virtual PipelineRuleFacility Facility { get; set; }

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
