namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Inbound record from Cargo Wise
    /// </summary>
    public class InboundRecord : BaseInboundRecord
    {
        /// <summary>
        /// Gets or sets the FTA Recon
        /// </summary>
        public virtual FtaRecon FtaRecon { get; set; }

        /// <summary>
        /// Gets or sets the Value Recon
        /// </summary>
        public virtual ValueRecon ValueRecon { get; set; }
    }
}
