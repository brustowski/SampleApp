using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.TruckExport.RuleConsignee
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
            Sheet("Consignee Rule");

            Map(p => p.ConsigneeCode, "Consignee");
            Map(p => p.Country, "Country of Destination");
            Map(p => p.Destination, "Destination");
            Map(p => p.UltimateConsigneeType, "Ultimate Consignee Type");
        }
    }
}
