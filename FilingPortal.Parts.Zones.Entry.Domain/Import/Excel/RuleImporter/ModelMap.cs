using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Zones.Entry.Domain.Import.Excel.RuleImporter
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
            Sheet("Importer Rule");

            Map(p => p.ImporterCode, "Importer");
            Map(p => p.Rlf, "RLF"); //Declaration tab
            Map(p => p.F3461Box29, "3461 Box 29"); // Declaration tab
            Map(p => p.Consignee, "Consignee"); // Declaration and Invoice header tab
            Map(p => p.Manufacturer, "Manufacturer"); // Invoice header tab
            Map(p => p.Supplier, "Supplier"); // Invoice header tab
            Map(p => p.Seller, "Seller"); // Invoice header tab
            Map(p => p.SoldToParty, "Sold To Party"); // Invoice header tab
            Map(p => p.ShipToParty, "Ship To Party"); // Invoice header tab
            Map(p => p.GoodsDescription, "Goods Description"); // Invoice lines tab
            Map(p => p.ReconIssue, "ReconIssue"); // Misc tab
        }
    }
}
