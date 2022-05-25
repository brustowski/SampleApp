using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.VesselExport.RuleUsppiConsignee
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
            Sheet("USPPI-Consignee Rule");

            Map(p => p.Usppi, "USPPI");
            Map(p => p.Consignee, "Consignee");
            Map(p => p.TransactionRelated, "Transaction Related");
            Map(p => p.UltimateConsigneeType, "Ultimate Consignee Type");
            Map(p => p.ConsigneeAddress, "Consignee Address");
        }
    }
}
