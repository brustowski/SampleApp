using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.TruckExport.RuleExporterConsignee
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
            Map(p => p.Address, "USPPI Address");
            Map(p => p.Contact, "Contact");
            Map(p => p.Phone, "Phone");
            Map(p => p.TranRelated, "Tran Related");
        }
    }
}
