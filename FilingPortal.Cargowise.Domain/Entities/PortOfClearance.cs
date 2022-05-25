using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities
{
    /// <summary>
    /// Describes port of clearance
    /// </summary>
    public class PortOfClearance : Entity
    {
        /// <summary>
        /// Gets or sets code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the office name
        /// </summary>
        public string Office { get; set; }

        /// <summary>
        /// Gets or sets the province
        /// </summary>
        public string Province { get; set; }
    }
}
