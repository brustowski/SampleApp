using FilingPortal.Domain.Entities;
using Framework.Domain;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Defines port rule
    /// </summary>
    public class RulePort : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Port of Clearance
        /// </summary>
        public string PortOfClearance { get; set; }
        /// <summary>
        /// Gets or sets sub-location
        /// </summary>
        public string SubLocation { get; set; }
        /// <summary>
        /// Gets or sets First port of arrival
        /// </summary>
        public string FirstPortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets final destination
        /// </summary>
        public string FinalDestination { get; set; }
    }
}
