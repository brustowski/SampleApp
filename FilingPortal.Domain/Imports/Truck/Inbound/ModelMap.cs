using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Truck;

namespace FilingPortal.Domain.Imports.Truck.Inbound
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="ImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<ImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Sheet("Batch Log");

            Map(p => p.Importer, "Importer");
            Map(p => p.Supplier, "Supplier");
            Map(p => p.PAPs, "PAPs");
        }
    }
}
