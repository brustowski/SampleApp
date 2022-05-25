namespace FilingPortal.Parts.Common.Domain.Common.InboundTypes
{
    /// <summary>
    /// Public interface for Auto-filing records
    /// </summary>
    public interface IAutoFilingEntity
    {
        /// <summary>
        /// Gets or sets value that indicates that this is update record
        /// </summary>
        bool IsUpdate { get; set; }
        /// <summary>
        /// Gets or sets whether inbound record is marked for auto refile
        /// </summary>
        bool IsAuto { get; set; }

        /// <summary>
        /// Gets or sets whether inbound record is auto processed for auto refile
        /// </summary>
        bool IsAutoProcessed { get; set; }

        /// <summary>
        /// Gets autofile config Importer or Exporter
        /// </summary>
        string AutoFileConfigId { get; }
    }
}
