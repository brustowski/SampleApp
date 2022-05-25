using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Entities
{
    /// <summary>
    /// Defines Vendor rule
    /// </summary>
    public class RuleVendor : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets vendor id
        /// </summary>
        public Guid VendorId { get; set; }
        /// <summary>
        /// Gets or sets vendor
        /// </summary>
        public virtual Client Vendor { get; set; }
        /// <summary>
        /// Gets or sets importer id
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets exporter id
        /// </summary>
        public Guid ExporterId { get; set; }
        /// <summary>
        /// Gets or sets Exporter
        /// </summary>
        public virtual Client Exporter { get; set; }
        /// <summary>
        /// Gets or sets the Export State
        /// </summary>
        public string ExportState { get; set; }
        /// <summary>
        /// Gets or sets Direct Ship place
        /// </summary>
        public string DirectShipPlace { get; set; }
        /// <summary>
        /// Gets or sets No. Packages
        /// </summary>
        public int? NoPackages { get; set; }
        /// <summary>
        /// Gets or sets the Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets ORG state
        /// </summary>
        public string OrgState { get; set; }
    }
}
