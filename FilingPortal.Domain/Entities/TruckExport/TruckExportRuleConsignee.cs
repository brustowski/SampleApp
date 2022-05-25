using System;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.TruckExport
{
    /// <summary>
    /// Defines the Export Truck Exporter Consignee Rule
    /// </summary>
    public class TruckExportRuleConsignee : AuditableEntity, IRuleEntity
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
