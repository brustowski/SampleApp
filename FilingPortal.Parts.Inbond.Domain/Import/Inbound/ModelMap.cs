using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Inbond.Domain.Import.Inbound
{
    /// <summary>
    /// Provides Excel file data mapping on Inbond Import Model
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Batch Log");

            Map(p => p.FirmsCode, "FIRMS Code");
            Map(p => p.ImporterCode, "Importer");
            Map(p => p.PortOfArrival, "Port of Arrival");
            Map(p => p.PortOfDestination, "Port of Destination");
            Map(p => p.ExportConveyance, "Conveyance/Truck Reg No");
            Map(p => p.ConsigneeCode, "Consignee");
            Map(p => p.Carrier, "Carrier");
            Map(p => p.Value, "Value");
            Map(p => p.ManifestQty, "Manifest qty");
            Map(p => p.ManifestQtyUnit, "Manifest qty UQ");
            Map(p => p.Weight, "Weight");
        }
    }
}
