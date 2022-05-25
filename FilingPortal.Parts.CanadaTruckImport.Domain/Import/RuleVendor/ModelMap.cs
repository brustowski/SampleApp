using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleVendor
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
            Sheet("Vendor Rule");

            Map(p => p.Vendor, "Vendor");
            Map(p => p.Importer, "Importer");
            Map(p => p.Exporter, "Exporter");
            Map(p => p.ExportState, "Export State");
            Map(p => p.DirectShipPlace, "Direct Ship Place");
            Map(p => p.NoPackages, "No Packages");
            Map(p => p.CountryOfOrigin, "Country Of Origin");
            Map(p => p.OrgState, "Org State");
        }
    }
}
