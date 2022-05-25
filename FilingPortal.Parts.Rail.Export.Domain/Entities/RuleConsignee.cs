using FilingPortal.Domain.Entities;
using Framework.Domain;

namespace FilingPortal.Parts.Rail.Export.Domain.Entities
{
    /// <summary>
    /// Defines the Consignee Rule
    /// </summary>
    public class RuleConsignee : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }

        /// <summary>
        /// Gets or sets the destination
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
    }
}
