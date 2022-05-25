using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Rail.Export.Domain.Import.Inbound
{

    /// <summary>
    /// Provides Excel file data mapping on inbound import
    /// </summary>
    internal class InboundImportModelMap : ParseModelMap<InboundImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundImportModel"/> class.
        /// </summary>
        public InboundImportModelMap()
        {
            Sheet("US Rail Export");

            Map(p => p.GroupId, "Rail Unique ID");
            Map(p => p.Exporter, "USPPI (Exporter)");
            Map(p => p.Importer, "Importer");
            Map(p => p.MasterBill, "Master Bill");
            Map(p => p.ContainerNumber, "Container Number");
            Map(p => p.LoadPort, "Load Port");
            Map(p => p.ExportPort, "Export Port");
            Map(p => p.Carrier, "Carrier");
            Map(p => p.TariffType, "Tariff Type");
            Map(p => p.Tariff, "Tariff");
            Map(p => p.GoodsDescription, "Goods Des.");
            Map(p => p.CustomsQty, "Customs Qty");
            Map(p => p.Price, "Price");
            Map(p => p.GrossWeight, "Gross Wght");
            Map(p => p.GrossWeightUOM, "Gross Wght UOM");
            Map(p => p.LoadDate, "Load Date");
            Map(p => p.ExportDate, "Export Date");
            Map(p => p.TerminalAddress, "Terminal Address");
        }
    }
}
