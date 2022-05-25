using System.Collections.Generic;

namespace FilingPortal.Domain.Entities.Pipeline
{
    using Framework.Domain;

    /// <summary>
    /// Provides the Pipeline Rule Batch Code Entity
    /// </summary>
    public class PipelineRuleBatchCode : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Batch Code
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        /// Gets or sets the Product
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Corresponding pipeline price rules
        /// </summary>
        public virtual ICollection<PipelineRulePrice> PipelinePriceRules { get; set; }
    }
}
