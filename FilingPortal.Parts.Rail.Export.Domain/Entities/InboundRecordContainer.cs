using System.ComponentModel.DataAnnotations.Schema;
using Framework.Domain;

namespace FilingPortal.Parts.Rail.Export.Domain.Entities
{
    /// <summary>
    /// Inbound container
    /// </summary>
    public class InboundRecordContainer: Entity
    {
        /// <summary>
        /// Gets or set container number
        /// </summary>
        public string ContainerNumber { get; set; }
        /// <summary>
        /// Gets or sets the Customs Qty
        /// </summary>
        public decimal CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the Gross Weight
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }
        /// <summary>
        /// Gets or sets container type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets inbound record id
        /// </summary>
        public int InboundRecordId { get; set; }
    }
}