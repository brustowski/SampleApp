using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.RuleExportConsignee
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

            Map(p => p.Exporter, "USPPI");
            Map(p => p.ConsigneeCode, "Consignee");
            Map(p => p.Address, "Pickup address");
            Map(p => p.Contact);
            Map(p => p.Phone);
            Map(p => p.TranRelated, "Tran Related");
        }
    }
}
