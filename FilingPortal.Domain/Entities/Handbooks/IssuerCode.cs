using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Describes issuer code entity
    /// </summary>
    public class IssuerCode : Entity
    {
        /// <summary>
        /// Gets or sets code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the transportation mode
        /// </summary>
        public string TransportationMode { get; set; }
    }
}
