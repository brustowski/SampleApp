using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Inbound record entity
    /// </summary>
    /// <typeparam name="TFilingHeader"></typeparam>
    public abstract class InboundEntity<TFilingHeader> : AuditableEntity, IInboundType where TFilingHeader : BaseFilingHeader
    {
        /// <summary>
        /// Gets or sets record soft delete flag
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// Gets or sets FilingHeaders for current model
        /// </summary>
        public virtual ICollection<TFilingHeader> FilingHeaders { get; set; } = new List<TFilingHeader>();
    }
}
