using System;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.VesselExport
{
    /// <summary>
    /// Defines the Export Vessel Usppi Consignee Rule
    /// </summary>
    public class VesselExportRuleUsppiConsignee : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets the USPPI Id
        /// </summary>
        public Guid UsppiId { get; set; }

        /// <summary>
        /// Gets or sets the USPPI
        /// </summary>
        public virtual Client Usppi { get; set; }

        /// <summary>
        /// Gets or sets the Consignee Code
        /// </summary>
        public Guid ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public virtual Client Consignee { get; set; }
        /// <summary>
        /// Gets or sets the transactions related
        /// </summary>
        public string TransactionRelated { get; set; }

        /// <summary>
        /// Get or sets Ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
        
        /// <summary>
        /// Gets or sets the address id
        /// </summary>
        public Guid? ConsigneeAddressId { get; set; }

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public virtual ClientAddress ConsigneeAddress { get; set; }
    }
}
