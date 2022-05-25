using Framework.Domain;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Represents the Base Status entity
    /// </summary>
    public abstract class BaseStatus<TId> : EntityWithTypedId<TId>
    {
        /// <summary>
        /// Gets or sets the status name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the shorthand status code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the status description
        /// </summary>
        public string Description { get; set; }
    }
}
