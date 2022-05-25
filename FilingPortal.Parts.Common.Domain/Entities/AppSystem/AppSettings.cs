using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.AppSystem
{
    /// <summary>
    /// Application settings parameter entity
    /// </summary>
    public class AppSettings : EntityWithTypedId<string>
    {
        /// <summary>
        /// Gets or sets settings section
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// Gets or sets settings description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets settings value
        /// </summary>
        public string Value { get; set; }
    }
}
