using FilingPortal.Domain.Enums;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Web.Models.Rail
{
    /// <summary>
    /// Represents the Freight complex value
    /// </summary>
    public class RailRuleFreightComposite
    {
        /// <summary>
        /// Gets or sets Freight
        /// </summary>
        public decimal? Freight { get; set; }

        /// <summary>
        /// Gets or sets Freight Type
        /// </summary>
        public FreightType FreightType { get; set; }

        /// <summary>
        /// Gets or sets Freight Type description
        /// </summary>
        public string FreightTypeDescription => FreightType.GetDescription();
    }
}