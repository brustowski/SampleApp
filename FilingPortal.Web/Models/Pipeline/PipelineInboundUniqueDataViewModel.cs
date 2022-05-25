namespace FilingPortal.Web.Models.Pipeline
{
    /// <summary>
    /// Represents Pipeline Unique Data View model
    /// </summary>
    public class PipelineInboundUniqueDataViewModel
    {
        
        /// <summary>
        /// Gets or sets the identifier of Pipeline Inbound Record Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Filing Header Id
        /// </summary>
        public int FilingHeaderId { get; set; }

        /// <summary>
        /// Gets or sets the Importer
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
        /// Gets or sets Site name for inbound record
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Gets or sets Facility for inbound record
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets or sets Quantity
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Gets or sets API
        /// </summary>
        public string API { get; set; }
        /// <summary>
        /// Gets or sets Export Date
        /// </summary>
        public string ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public string ImportDate { get; set; }
    }
}