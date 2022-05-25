using System;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    /// <summary>
    /// Represents Pipeline Inbound Record Item
    /// </summary>
    public class PipelineInbound : InboundEntity<PipelineFilingHeader>
    {
        /// <summary>
        /// Gets or sets Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets Batch
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// Gets or sets Ticket Number
        /// </summary>
        public string TicketNumber { get; set; }
        /// <summary>
        /// Gets or sets Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Gets or sets API
        /// </summary>
        public decimal API { get; set; }
        /// <summary>
        /// Gets or sets Export Date
        /// </summary>
        public DateTime ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public DateTime ImportDate { get; set; }
        /// <summary>
        /// Gets or sets Site name for inbound record
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Gets or sets Facility for inbound record
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets ot sets Entry Number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets pipeline inbound documents collection
        /// </summary>
        public virtual ICollection<PipelineDocument> Documents { get; set; }
    }
}
