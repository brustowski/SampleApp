using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Admin.AutoCreate
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Auto-Create records");

            Map(p => p.ShipmentType, "Shipment Type");
            Map(p => p.EntryType, "Entry Type");
            Map(p => p.TransportMode, "Transport Mode");
            Map(p => p.ImporterExporter, "Importer/Exporter");
            Map(p => p.AutoCreate, "Auto-Create");
        }
    }
}
