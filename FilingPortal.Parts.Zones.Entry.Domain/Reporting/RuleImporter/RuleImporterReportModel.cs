using System;

namespace FilingPortal.Parts.Zones.Entry.Domain.Reporting.RuleImporter
{
    /// <summary>
    /// Defines Importer rule Report model
    /// </summary>
    public class RuleImporterReportModel
    {
        /// <summary>
        /// Rule ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string ImporterCode { get; set; }

        /// <summary>
        /// Gets or sets RLF
        /// </summary>
        public string Rlf { get; set; } //Declaration tab
        /// <summary>
        /// Gets or sets 3461 box 29
        /// </summary>
        public string F3461Box29 { get; set; } // Declaration tab
        
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
        /// <summary>
        /// Gets or sets Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets Rule Creator
        /// </summary>
        public string CreatedUser { get; set; }
    }
}
