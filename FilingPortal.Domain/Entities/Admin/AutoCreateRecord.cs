using Framework.Domain;

namespace FilingPortal.Domain.Entities.Admin
{
    /// <summary>
    /// Describes the Auto-create entity
    /// </summary>
    public class AutoCreateRecord : AuditableEntity, IRuleEntity
    {
        /// <summary>
        /// Gets or sets Shipment Type
        /// </summary>
        public string ShipmentType { get; set; }
        /// <summary>
        /// Gets or sets entry type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets Transport Mode
        /// </summary>
        public string TransportMode { get; set; }
        /// <summary>
        /// Gets or sets Importer/Exporter
        /// </summary>
        public string ImporterExporter { get; set; }
        /// <summary>
        /// Gets or sets whether AutoCreate or not
        /// </summary>
        public bool AutoCreate { get; set; }
    }
}
