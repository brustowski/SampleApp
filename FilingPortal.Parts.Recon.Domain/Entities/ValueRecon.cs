using FilingPortal.Parts.Recon.Domain.Enums;
using Framework.Domain;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Represents the Value Recon entity
    /// </summary>
    public class ValueRecon : AuditableEntity
    {
        #region Client Data
        /// <summary>
        /// Get or sets the Final Unit Price
        /// </summary>
        public decimal? FinalUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the Final Total Value
        /// </summary>
        public decimal? FinalTotalValue { get; set; }

        /// <summary>
        /// Gets or sets the client note
        /// </summary>
        public string ClientNote { get; set; } 
        #endregion

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public int Status { get; set; } = (int)ValueReconStatusValue.Open;

        /// <summary>
        /// Gets or sets the Inbound record
        /// </summary>
        public virtual InboundRecord Inbound { get; set; }
    }
}
