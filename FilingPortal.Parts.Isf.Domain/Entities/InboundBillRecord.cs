using Framework.Domain;

namespace FilingPortal.Parts.Isf.Domain.Entities
{
    /// <summary>
    /// Entity, representing inbound bill record
    /// </summary>
    public class InboundBillRecord : AuditableEntity
    {
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Gets or sets Inbound record
        /// </summary>
        public virtual InboundRecord Inbound { get; set; }
        /// <summary>
        /// Gets or sets bill type
        /// </summary>
        public string BillType { get; set; }

        /// <summary>
        /// Gets or sets bill number
        /// </summary>
        public string BillNumber { get; set; }
    }
}
