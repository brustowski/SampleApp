using Framework.Domain;

namespace FilingPortal.Domain.Entities.Handbooks
{
    /// <summary>
    /// Represents the Transport Mode entry 
    /// </summary>
    public class TransportMode : Entity
    {
        /// <summary>
        /// Gets or sets the Code Number
        /// </summary>
        public string CodeNumber { get; set; }
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Country
        /// </summary>
        public string Country { get; set; }
        public string ServiceCode { get; set; }
        public string ContainerCode { get; set; }
    }
}
