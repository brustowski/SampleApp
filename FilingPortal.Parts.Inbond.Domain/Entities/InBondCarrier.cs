using Framework.Domain;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Represents the In-Bond Carrier handbook entry
    /// </summary>
    public class InBondCarrier : EntityWithTypedId<string>
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
    }
}
