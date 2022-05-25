using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Admin.AutoCreate
{
    /// <summary>
    /// Provides Excel file data mapping on Auto-Create record Import model
    /// </summary>
    internal class ImportModel : ParsingDataModel
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
