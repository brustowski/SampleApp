using System;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Pipeline
{
    /// <summary>
    /// Represents Pipeline Inbound Read model Record Item
    /// </summary>
    public class PipelineInboundReadModel : InboundReadModelOld
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
        /// Gets or sets the model Mapping Status title
        /// </summary>
        public string MappingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets the model Filing status title
        /// </summary>
        public string FilingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Importer rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasImporterRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Batch rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasBatchRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Facility rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasFacilityRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has Price rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasPriceRule { get; set; }
    }
}
