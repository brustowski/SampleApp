using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Base class for inbounf record unique data
    /// </summary>
    public abstract class BaseFilingData: Entity
    {
        /// <summary>
        /// Gets or sets Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }
    }
}
