using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities
{
    /// <summary>
    /// Represents the Job Status entity
    /// </summary>
    public class HeaderJobStatus : Entity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets status code
        /// </summary>
        public string Code { get; set; }
    }
}
