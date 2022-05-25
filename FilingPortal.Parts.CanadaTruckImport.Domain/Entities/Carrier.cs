using Framework.Domain;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Represents the Carrier entity 
    /// </summary>
    public class Carrier : EntityWithTypedId<string>
    {
        /// <summary>
        /// Get or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Transport Mode
        /// </summary>
        public string TransportMode { get; set; }
    }
}
