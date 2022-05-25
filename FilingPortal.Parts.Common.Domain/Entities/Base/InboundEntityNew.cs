using System;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Inbound record entity
    /// </summary>
    /// <typeparam name="TFilingHeader"></typeparam>
    public abstract class InboundEntityNew<TFilingHeader> : InboundEntity<TFilingHeader>, IAutoFilingEntity, IValidationRequiredEntity where TFilingHeader : BaseFilingHeader
    {
        /// <summary>
        /// Gets or sets value that indicates that this is update record
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        /// Gets or sets whether inbound record is marked for auto refile
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// Gets or sets whether inbound record is auto processed for auto refile
        /// </summary>
        public bool IsAutoProcessed { get; set; }

        /// <summary>
        /// Gets autofile config Importer or Exporter
        /// </summary>
        public abstract string AutoFileConfigId { get; }

        /// <summary>
        /// Gets or sets value that indicates that record is valid or not
        /// </summary>
        public bool ValidationPassed { get; set; }

        /// <summary>
        /// Gets or sets the validation results value
        /// </summary>
        public string ValidationResult { get; set; }
        /// <summary>
        /// Gets or sets Modified Date
        /// </summary>
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets Modified user
        /// </summary>
        public string ModifiedUser { get; set; }
    }
}
