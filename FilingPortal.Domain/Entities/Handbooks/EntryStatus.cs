using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Represents the Entry Status entity
    /// </summary>
    public class EntryStatus : EntityWithTypedId<int>
    {
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the status type
        /// </summary>
        public string StatusType { get; set; }
    }
}
