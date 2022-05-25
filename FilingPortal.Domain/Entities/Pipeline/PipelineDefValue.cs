using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    /// <summary>
    /// represents Pipeline Default Values
    /// </summary>
    public class PipelineDefValue : BaseDefValueWithSection<PipelineSection>, IConfigurationEntity
    {
        /// <summary>
        /// Gets or sets the Depends On field
        /// </summary>
        public virtual PipelineDefValue DependsOn { get; set; }
    }
}
