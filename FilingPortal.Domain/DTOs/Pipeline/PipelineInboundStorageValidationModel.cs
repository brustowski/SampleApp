using System;

namespace FilingPortal.Domain.DTOs.Pipeline
{
    /// <summary>
    /// Represents Pipeline Inbound Import data model for DB validation
    /// </summary>
    public class PipelineInboundStorageValidationModel
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
        /// Gets or sets corresponding row number in the file
        /// </summary>
        public int RowNumberInFile { get; set; }
        /// <summary>
        /// Gets or sets Site name for inbound record
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Gets or sets Facility for inbound record
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets or sets Entry number
        /// </summary>
        public string EntryNumber { get; set; }
    }
}
