using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Represents the Entry Type entry
    /// </summary>
    public class EntryType : EntityWithTypedId<string>
    {
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
    }
}
