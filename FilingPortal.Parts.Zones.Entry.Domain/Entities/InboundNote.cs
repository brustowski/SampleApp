using System;
using Framework.Domain;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents inbound record note
    /// </summary>
    public class InboundNote : Entity
    {
        /// <summary>
        /// Parent record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Parent inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }
        /// <summary>
        /// Gets or sets note title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets note date
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Gets or sets note message
        /// </summary>
        public string Message { get; set; }
    }
}
