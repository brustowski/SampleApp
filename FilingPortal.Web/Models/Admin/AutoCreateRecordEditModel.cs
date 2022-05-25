namespace FilingPortal.Web.Models.Admin
{
    /// <summary>
    /// Class representing Auto-Create record in Edit model
    /// </summary>
    public class AutoCreateRecordEditModel
    {
        /// <summary>
        /// Gets or sets the record identifier
        /// </summary>
        public int Id { get; set; }
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