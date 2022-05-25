using Framework.Domain;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Entities
{
    /// <summary>
    /// Entity for inbound XML files
    /// </summary>
    public class InboundXml : AuditableEntity
    {
        /// <summary>
        /// Gets or sets file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets file contents
        /// </summary>
        public byte[] Content { get; set; }
        /// <summary>
        /// Gets or sets file status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Gets or sets file validation result
        /// </summary>
        public string ValidationResult { get; set; }
    }
}
