using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.TruckExport;

namespace FilingPortal.Domain.Imports.TruckExport.Inbound
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="TruckExportImportModel"/>
    /// </summary>
    internal class TruckExportParseModelMap : ParseModelMap<TruckExportImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportParseModelMap"/> class.
        /// </summary>
        public TruckExportParseModelMap()
        {
            Sheet("Sheet1");

            Map(p => p.Exporter, "Exporter");
            Map(p => p.Importer, "Importer");
            Map(p => p.TariffType, "TariffType");
            Map(p => p.Tariff, "Tariff");
            Map(p => p.RoutedTran, "RoutedTran");
            Map(p => p.SoldEnRoute, "SoldEnRoute");
            Map(p => p.MasterBill, "CharterRefNum");
            Map(p => p.Origin, "Port Code");
            Map(p => p.Export, "ExportPort");
            Map(p => p.ExportDate, "Actual Border Crossing Date");
            Map(p => p.ECCN, "ECCN");
            Map(p => p.GoodsDescription, "Product");
            Map(p => p.CustomsQty, "Final Product Qty BBL");
            Map(p => p.Price, "Final Value");
            Map(p => p.GrossWeight, "Final KG");
            Map(p => p.GrossWeightUOM, "GrossWghtUOM");
            Map(p => p.Hazardous, "Hazardous");
            Map(p => p.OriginIndicator, "OriginIndicator");
            Map(p => p.GoodsOrigin, "GoodsOrigin");
        }
    }
}
