using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Framework.Domain;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents Importer rule entity
    /// </summary>
    public class RuleImporter : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public Client Importer { get; set; }
        /// <summary>
        /// Get or sets Importer Id
        /// </summary>
        public Guid ImporterId { get; set; }

        /// <summary>
        /// Gets or sets RLF
        /// </summary>
        public string Rlf { get; set; } //Declaration tab
        
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string Consignee { get; set; } // Declaration and Invoice header tab
        /// <summary>
        /// Gets or sets Manufacturer
        /// </summary>
        public string Manufacturer { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Supplier
        /// </summary>
        public string Supplier { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Seller
        /// </summary>
        public string Seller { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Sold to Party
        /// </summary>
        public string SoldToParty { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Ship to party
        /// </summary>
        public string ShipToParty { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Goods Description
        /// </summary>
        public string GoodsDescription { get; set; } // Invoice lines tab
        /// <summary>
        /// Gets or sets Recon Issue
        /// </summary>
        public string ReconIssue { get; set; } // Misc tab
    }
}
