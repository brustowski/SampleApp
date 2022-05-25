using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Inbond.Domain.Import.RuleEntry
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
            Sheet("Entry Rule");

            Map(p => p.FirmsCode, "FIRMsCode");
            Map(p => p.Importer, "Importer");
            Map(p => p.ImporterAddress, "Importer Address");
            Map(p => p.Carrier, "In-Bond Carrier");
            Map(p => p.Consignee, "Consignee");
            Map(p => p.ConsigneeAddress, "Consignee Address");
            Map(p => p.UsPortOfDestination, "US Port of Destination");
            Map(p => p.EntryType, "In-Bond Entry Type");
            Map(p => p.Shipper, "Shipper");
            Map(p => p.Tariff, "Tariff");
            Map(p => p.PortOfPresentation, "Port of Presentation");
            Map(p => p.ForeignDestination, "Foreign Destination");
            Map(p => p.TransportMode, "Transport Mode");
            Map(p => p.ConfirmationNeeded, "Confirmation Needed");
        }
    }
}
