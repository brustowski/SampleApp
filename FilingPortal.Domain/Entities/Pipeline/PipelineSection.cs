using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    /// <summary>
    /// Defines The Pipeline Section Entity
    /// </summary>
    public class PipelineSection: BaseSection
    {
        /// <summary>
        /// Gets or sets the Parent
        /// </summary>
        public virtual PipelineSection Parent { get; set; }

        /// <summary>
        /// Gets or sets the Descendants
        /// </summary>
        public virtual ICollection<PipelineSection> Descendants { get; set; }

        /// <summary>
        /// Gets or sets the field configurations
        /// </summary>
        public virtual ICollection<PipelineDefValue> Fields { get; set; }
    }
}
