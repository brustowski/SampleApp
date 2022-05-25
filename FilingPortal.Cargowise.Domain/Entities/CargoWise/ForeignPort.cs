using Framework.Domain;

namespace FilingPortal.Cargowise.Domain.Entities.CargoWise
{
    /// <summary>
    /// Defines Foreign port entity
    /// </summary>
    public class ForeignPort : Entity
    {
        /// <summary>
        /// Gets or sets Port code
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// Gets or sets UNLOCO
        /// </summary>
        public string UNLOCO { get; set; }
        /// <summary>
        /// Gets or sets Country
        /// </summary>
        public string Country { get; set; }
        
    }
}
