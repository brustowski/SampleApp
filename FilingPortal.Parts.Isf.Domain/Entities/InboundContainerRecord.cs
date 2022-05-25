using Framework.Domain;

namespace FilingPortal.Parts.Isf.Domain.Entities
{
    /// <summary>
    /// Entity, representing inbound container record
    /// </summary>
    public class InboundContainerRecord : AuditableEntity
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
        /// Gets or sets container type
        /// </summary>
        public string ContainerType { get; set; }

        /// <summary>
        /// Gets or sets container number
        /// </summary>
        public string ContainerNumber { get; set; }
    }
}
